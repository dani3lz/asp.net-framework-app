using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObject
{
     [BsonIgnoreExtraElements]
     public class BookModel
     {
          [BsonId]
          public ObjectId ID { get; set; }

          [BsonElement("name")]
          public string Name { get; set; }

          [BsonElement("autorul")]
          public string Autor { get; set; }

          [BsonElement("category")]
          public string Category { get; set; }

          [BsonElement("stock")]
          public bool Stock { get; set; }

          [BsonElement("img")]
          public string Img { get; set; }

          [BsonElement("pdf")]
          public string Pdf { get; set; }

          [BsonElement("price")]
          public string Price { get; set; }
          public int len { get; set; }
     }
}
