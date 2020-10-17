using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Movies
    {

    
       [BsonId]
    public ObjectId MovieId { get; set; }
    [BsonElement]


    public string MovieName { get; set; }

    [BsonElement]
    [Required]
    public string Actor { get; set; }

        [BsonElement]


        public string length { get; set; }

        [BsonElement]
       
        public string imageUrl { get; set; }
        public string Description { get; set; }

    [Display(Name = "Created Date")]
    [BsonElement]
    [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

}
}
