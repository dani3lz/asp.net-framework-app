using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace BusinessObject
{
     [BsonIgnoreExtraElements]
     public class UserProfil
     {
          [BsonId]
          public ObjectId ID { get; set; }

          [BsonElement("username")]
          public string Username { get; set; }

          [BsonElement("first")]
          public string First { get; set; }

          [BsonElement("last")]
          public string Last { get; set; }

          [BsonElement("adress")]
          public string Adresa { get; set; }

          [BsonElement("phone")]
          public string Telefonul { get; set; }


          [BsonElement("email")]
          public string Email { get; set; }


          public string ReEmail { get; set; }


          public string PasswordCurentMail { get; set; }

          public string PasswordCurentPass { get; set; }


          [BsonElement("password")]
          public string Password { get; set; }


          public string RePassword { get; set; }

          [BsonElement("class")]
          public string Class { get; set; }
     }

     








}
