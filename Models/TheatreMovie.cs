using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class TheatreMovie
    {
        [BsonId]
        public ObjectId TheatreMovieID { get; set; }


        public ObjectId TID { get; set; }

        public ObjectId MID { get; set; }

        public int seat { get; set; }
    }
}
