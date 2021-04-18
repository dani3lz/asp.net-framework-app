using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObject;

namespace BusinessObject
{
     public class ViewModel
     {
          public BookModel Book { get; set; }
          public List<BookModel> books { get; set; }
          public List<UserProfil> Users { get; set; }
          public UserData Category { get; set; }
     }
}
