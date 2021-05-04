using BusinessObject;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
     public class AccessDB
     {
          public List<UserProfil> GetUsers()
          {
               var client = new MongoClient("mongodb://localhost:27017");
               var db = client.GetDatabase("BOOKShop");
               var collection = db.GetCollection<UserProfil>("Users");
               var res = collection.Find(new BsonDocument()).ToList();
               return res;
          }

          public List<BookModel> GetBooks()
          {
               var client = new MongoClient("mongodb://localhost:27017");
               var db = client.GetDatabase("BOOKShop");
               var collection = db.GetCollection<BookModel>("Books");
               var res = collection.Find(new BsonDocument()).ToList();
               return res;
          }

          public string CheckAdmin(string user)
          {
               string Check = "False";
               var res = Connect("Users");
               foreach (var r in res)
               {
                    if (r.GetValue("username").ToString() == user)
                    {
                         if (r.GetValue("class").ToString() == "admin")
                         {
                              Check = "True";
                         }
                    }
               }
               return Check;
          }

          public string Post(List<BsonDocument> res, BookModel bk)
          {
               string Error = "";

               foreach (var r in res)
               {
                    if (bk.Name == r.GetValue("name").ToString())
                    {
                         return "Denumirea cartii deja exista!";
                    }
               }

               var document = new BsonDocument
               {
                    {"name", bk.Name },
                    {"autorul", bk.Autor },
                    {"category", bk.Category },
                    {"img", bk.Img },
                    {"pdf", bk.Pdf },
                    {"price", bk.Price }
               };

               var client = new MongoClient("mongodb://localhost:27017");
               var db = client.GetDatabase("BOOKShop");
               var collection = db.GetCollection<BsonDocument>("Books");
               collection.InsertOneAsync(document);
               Error = "Succes!";
               return Error;
          }

          public List<BsonDocument> Connect(string collectionDB)
          {
               var client = new MongoClient("mongodb://localhost:27017");
               var db = client.GetDatabase("BOOKShop");
               var collection = db.GetCollection<BsonDocument>(collectionDB);
               var res = collection.Find(new BsonDocument()).ToList();
               return res;
          }

          public bool Login(List<BsonDocument> res, UserLogin log)
          {
               bool Entered = false;
               foreach (var r in res)
               {
                    if (r.GetValue("username").ToString() == log.Username && r.GetValue("password").ToString() == log.Password)
                    {
                         Entered = true;
                         break;
                    }
               }
               return Entered;
          }

          public string Register(List<BsonDocument> res, UserRegistration reg, string collectionDB)
          {
               string Error = "";

               foreach (var r in res)
               {
                    if (reg.Username == r.GetValue("username"))
                    {
                         return "Username-ul deja exista!";
                    }
                    if (reg.Email == r.GetValue("email"))
                    {
                         return "Email-ul deja exista!";
                    }
               }

               if (reg.Password != reg.RePassword)
               {
                    return "Parolele sunt diferite!";
               }

               var document = new BsonDocument
               {
                    {"username", reg.Username },
                    {"email", reg.Email },
                    {"password", reg.Password },
                    {"first", "" },
                    {"last", "" },
                    {"address", "" },
                    {"phone", "" },
                    {"class", "user" }
               };

               var client = new MongoClient("mongodb://localhost:27017");
               var db = client.GetDatabase("BOOKShop");
               var collection = db.GetCollection<BsonDocument>(collectionDB);
               collection.InsertOneAsync(document);

               return Error;
          }

          public (string, int) ChangeProfile(UserProfil changes, string button, string collectionDB)
          {
               string Error = null;
               int nr = 0;

               var client = new MongoClient("mongodb://localhost:27017");
               var db = client.GetDatabase("BOOKShop");
               var users = db.GetCollection<BsonDocument>(collectionDB);

               var filter = Builders<BsonDocument>.Filter.Eq("username", changes.Username);

               if (button == "generalinfo")
               {
                    if (changes.First == null || changes.Last == null || changes.Telefonul == null || changes.Adresa == null)
                    {
                         Error = "Toate spatiile sunt obligatorii de completat!";
                         return (Error, nr);
                    }
                    else
                    {
                         var update1 = Builders<BsonDocument>.Update.Set("first", changes.First);
                         var update2 = Builders<BsonDocument>.Update.Set("last", changes.Last);
                         var update3 = Builders<BsonDocument>.Update.Set("phone", changes.Telefonul);
                         var update4 = Builders<BsonDocument>.Update.Set("address", changes.Adresa);

                         users.UpdateOne(filter, update1);
                         users.UpdateOne(filter, update2);
                         users.UpdateOne(filter, update3);
                         users.UpdateOne(filter, update4);
                         return (Error, nr);
                    }
               }

               if (button == "securitymail")
               {
                    var result = users.Find(new BsonDocument { { "username", changes.Username } }).ToList();
                    foreach (var r in result)
                    {
                         if (changes.Email == r.GetValue("email").ToString() && changes.PasswordCurentMail == r.GetValue("password").ToString())
                         {
                              var update = Builders<BsonDocument>.Update.Set("email", changes.ReEmail);
                              users.UpdateOne(filter, update);
                              return (Error, nr);
                         }
                         else
                         {
                              if (changes.Email != r.GetValue("email").ToString() && changes.Email != null)
                              {
                                   Error = "Email-ul curent nu coincide";
                                   nr = 1;
                                   return (Error, nr);
                              }
                              else
                              {
                                   if (changes.PasswordCurentMail != r.GetValue("password").ToString() && changes.PasswordCurentMail != null)
                                   {
                                        Error = "Parola curenta nu coincide";
                                        nr = 1;
                                        return (Error, nr);
                                   }
                                   else
                                   {
                                        Error = "Toate spatiile sunt obligatorii de completat!";
                                        nr = 1;
                                        return (Error, nr);
                                   }
                              }
                         }
                    }
               }

               if (button == "securitypass")
               {
                    var result = users.Find(new BsonDocument { { "username", changes.Username } }).ToList();
                    foreach (var r in result)
                    {
                         if (changes.PasswordCurentPass == r.GetValue("password").ToString() && changes.Password == changes.RePassword)
                         {
                              var update = Builders<BsonDocument>.Update.Set("password", changes.Password);
                              users.UpdateOne(filter, update);
                              return (Error, nr);
                         }
                         else
                         {
                              if (changes.PasswordCurentPass != r.GetValue("password").ToString() && changes.PasswordCurentPass != null)
                              {
                                   Error = "Parola curenta nu coincide!";
                                   nr = 2;
                                   return (Error, nr);
                              }
                              else
                              {
                                   if (changes.Password != changes.RePassword && changes.Password != null && changes.RePassword != null)
                                   {
                                        Error = "Parola noua nu coincide!";
                                        nr = 2;
                                        return (Error, nr);
                                   }
                                   else
                                   {
                                        Error = "Toate spatiile sunt obligatorii de completat!";
                                        nr = 2;
                                        return (Error, nr);
                                   }
                              }
                         }
                    }

               }
               return (Error, nr);
          }





     }
}
