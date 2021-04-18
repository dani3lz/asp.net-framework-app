using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using BusinessObject;
using MongoDB.Bson;

namespace BusinessLogic
{
     public class UserBL
     {
          public List<UserProfil> GetUsers()
          {
               return new AccessDB().GetUsers();
          }

          public List<BookModel> GetBooks()
          {
               return new AccessDB().GetBooks();
          }

          public string CheckAdmin(string user)
          {
               return new AccessDB().CheckAdmin(user);
          }
          public string Post(List<BsonDocument> res, BookModel bk)
          {
               return new AccessDB().Post(res, bk);
          }
          public List<BsonDocument> Connect(string collectionDB)
          {
               return new AccessDB().Connect(collectionDB);
          }

          public string Register(string collectionDB, List<BsonDocument> res, UserRegistration reg)
          {
               return new AccessDB().Register(res, reg, collectionDB);
          }

          public bool Login(List<BsonDocument> res, UserLogin log)
          {
               return new AccessDB().Login(res, log);
          }

          public (string, int) ChangeProfile(UserProfil changes, string button, string collectionDB)
          {
               return new AccessDB().ChangeProfile(changes, button, collectionDB);
          }
     }
}
