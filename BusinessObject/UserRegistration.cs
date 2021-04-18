using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObject
{
     public class UserRegistration
     {
          [BsonId]
          public int UserID { get; set; }

          [Required(ErrorMessage = "Introduceti username-ul!")]
          public string Username { get; set; }

          [Required(ErrorMessage = "Introduceti email-ul!")]
          public string Email { get; set; }

          [Required(ErrorMessage = "Introduceti parola!")]
          public string Password { get; set; }

          [Required(ErrorMessage = "Introduceti parola din nou!")]
          [Compare("Password", ErrorMessage = "Parola nu coincide!")]
          public string RePassword { get; set; }

     }
}
