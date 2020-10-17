using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Movie
    {

    
      
    public string MovieId { get; set; }
    


    public string MovieName { get; set; }

    
    public string Actor { get; set; }

      


        public string length { get; set; }

        
        public string imageUrl { get; set; }
        public string Description { get; set; }

   
   
    public DateTime CreatedDate { get; set; }

}
}
