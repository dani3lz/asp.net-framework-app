using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using BusinessLogic;
using BusinessObject;

namespace eUseControl.Web.Controllers
{
     public class MarketController : Controller
     {

          public ActionResult Books()
          {
               int count = 0;
               bool exist = false;
               var ctg = Request.QueryString["ctg"]; // ?ctg=
               var result = new UserBL().Connect("Category");
               foreach (var r in result)
               {
                    if(r.GetValue("name").ToString() == ctg)
                    {
                         exist = true;
                    }
               }

               if (exist)
               {
                    UserData u = new UserData();
                    u.Category = new List<string> { };
                    u.Category.Add(ctg);
                    var booksDB = new UserBL().Connect("Books");
                    u.Books = new List<string> { };
                    u.Img = new List<string> { };
                    u.Price = new List<string> { };
                    foreach (var r in booksDB)
                    {
                         if (r.GetValue("category").ToString() == ctg)
                         {
                              u.Books.Add(r.GetValue("name").ToString());
                              u.Img.Add(r.GetValue("img").ToString());
                              u.Price.Add(r.GetValue("price").ToString());
                              count++;
                         }
                    }
                    u.Lenght = count;
                    return View(u);
               }
               else
               {
                    return RedirectToAction("Index", "Home");
               }
          }

          [HttpGet]
          public ActionResult Category()
          {
               UserData u = new UserData();
               var result = new UserBL().Connect("Category");
               u.Category = new List<string> { };
               foreach (var r in result)
               {
                    u.Category.Add(r.GetValue("name").ToString());
               }
               return View(u);
          }

          [HttpPost]
          public ActionResult Category(string btn)
          {
               return RedirectToAction("Books", "Market", new { @ctg = btn });
          }
     }
}