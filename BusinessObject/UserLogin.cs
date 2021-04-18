﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObject
{
     public class UserLogin
     {
          [BsonId]
          public string ID { get; set; }

          [BsonElement("username")]
          [Required(ErrorMessage = "Introduceti username-ul!")]
          public string Username { get; set; }

          [BsonElement("password")]
          [Required(ErrorMessage = "Introduceti parola!")]
          [DataType(DataType.Password)]
          public string Password { get; set; }

          [Display(Name = "Remember me")]
          public bool Remember { get; set; }
     }
}
