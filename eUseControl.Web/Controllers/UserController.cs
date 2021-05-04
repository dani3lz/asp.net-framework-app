using System.Collections.Generic;
using System.Web.Mvc;
using BusinessLogic;
using BusinessObject;
using System.IO;
using System.Web;
using System;

namespace eUseControl.Web.Controllers
{
     public class UserController : Controller
     {
          [HttpGet]
          public ActionResult AdminPanel()
          {
               if (Session["Username"] != null)
               {
                    TempData["Check"] = new UserBL().CheckAdmin(Session["Username"].ToString());
                    if (TempData["Check"].ToString() == "True")
                    {
                         ViewModel dy = new ViewModel();
                         dy.Users = new UserBL().GetUsers();
                         dy.Category = new UserData();
                         var result = new UserBL().Connect("Category");
                         dy.Category.Category = new List<string> { };
                         foreach (var r in result)
                         {
                              dy.Category.Category.Add(r.GetValue("name").ToString());
                         }
                         return View(dy);
                    }
               }
               return RedirectToAction("Index", "Home");
          }


          [HttpPost]
          public ActionResult AdminPanel(ViewModel bk, string btn, string select, HttpPostedFileBase ImageFile, HttpPostedFileBase PdfFile)
          {
               if (btn == "post")
               {
                    if (bk.Book.Name == null || bk.Book.Autor == null || bk.Book.Price == null)
                    {
                         TempData["Error"] = "Toate spatiile sunt obligatorii de completat!";
                         return RedirectToAction("AdminPanel", "User", new { @option = "addbook" });
                    }
                    else
                    {
                         var res = new UserBL().Connect("Books");
                         bk.Book.Category = select;
                         try
                         {
                              // IMG
                              string filename = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                              string extension = Path.GetExtension(ImageFile.FileName);
                              if(extension != ".jpg")
                              {
                                   TempData["Error"] = "Formatul imaginii trebuie sa fie jpg! / " + extension;
                                   return RedirectToAction("AdminPanel", "User", new { @option = "addbook" });
                              }
                              filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                              bk.Book.Img = filename;
                              string ImagePath = "~/Content/Books/img/" + filename;
                              filename = Path.Combine(Server.MapPath("~/Content/Books/img/"), filename);
                              ImageFile.SaveAs(filename);

                              // PDF
                              string pdfname = Path.GetFileNameWithoutExtension(PdfFile.FileName);
                              string pdfextension = Path.GetExtension(PdfFile.FileName);
                              if(pdfextension != ".pdf")
                              {
                                   TempData["Error"] = "Formatul nu este PDF!";
                                   return RedirectToAction("AdminPanel", "User", new { @option = "addbook" });
                              }

                              pdfname = pdfname + DateTime.Now.ToString("yymmssfff") + pdfextension;
                              bk.Book.Pdf = pdfname;
                              string PdfPath = "~/Content/Books/pdf/" + pdfname;
                              pdfname = Path.Combine(Server.MapPath("~/Content/Books/pdf/"), pdfname);
                              PdfFile.SaveAs(pdfname);
                         }
                         catch (Exception)
                         {
                              TempData["Error"] = "Toate spatiile sunt obligatorii de completat!";
                              return RedirectToAction("AdminPanel", "User", new { @option = "addbook" });
                         }

                         string Error = new UserBL().Post(res, bk.Book);
                         TempData["Error"] = Error;
                         return RedirectToAction("AdminPanel", "User", new { @option = "addbook" });
                    }
               }
               return RedirectToAction("AdminPanel", "User", new { @option = btn });
          }


          [HttpGet]
          public ActionResult Profile()
          {
               if (Session["Username"] != null)
               {
                    UserProfil u = new UserProfil();
                    var result = new UserBL().Connect("Users");
                    foreach(var r in result)
                    {
                         if(r.GetValue("username").ToString() == Session["Username"].ToString())
                         {
                              u.Email = r.GetValue("email").ToString();
                              u.First = r.GetValue("first").ToString();
                              u.Last = r.GetValue("last").ToString();
                              u.Adresa = r.GetValue("address").ToString();
                              u.Telefonul = r.GetValue("phone").ToString();
                         }
                    }
                    TempData["Check"] = new UserBL().CheckAdmin(Session["Username"].ToString());
                    return View(u);
               }
               else
               {
                    return RedirectToAction("Index", "Home");
               }
          }

          [HttpPost]
          public ActionResult Profile(UserProfil changes, string button)
          {
               if (Session["Username"] != null)
               {
                    changes.Username = Session["Username"].ToString();
                    TempData["Check"] = new UserBL().CheckAdmin(Session["Username"].ToString());
                    if (button != "adm")
                    {
                         if (button == "securitymail" || button == "generalinfo" || button == "securitypass")
                         {
                              (string Error, int nr) = new UserBL().ChangeProfile(changes, button, "Users");
                              TempData["Succes"] = null;
                              // Errors
                              if (nr == 0)
                              {
                                   if (Error == null)
                                   {
                                        TempData["Succes"] = "Modificarile au fost salvate!";
                                   }
                                   else
                                   {
                                        TempData["ErrorGeneral"] = Error;
                                   }
                              }
                              else
                              {
                                   if (nr == 1)
                                   {
                                        if (Error == null)
                                        {
                                             TempData["Succes"] = "Modificarile au fost salvate!";
                                        }
                                        else
                                        {
                                             TempData["ErrorMail"] = Error;
                                        }
                                   }
                                   else
                                   {
                                        if (nr == 2)
                                        {
                                             if (Error == null)
                                             {
                                                  TempData["Succes"] = "Modificarile au fost salvate!";
                                             }
                                             else
                                             {
                                                  TempData["ErrorPass"] = Error;
                                             }
                                        }
                                   }
                              }
                              return RedirectToAction("Profile", "User", new { @option = Request.QueryString["option"] });
                         }
                         return RedirectToAction("Profile", "User", new { @option = button });
                    }
                    else
                    {
                         return RedirectToAction("AdminPanel", "User");
                    }

                    
               }
               return View("Index", "Home");
          }
     }

}