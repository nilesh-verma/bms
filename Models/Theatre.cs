using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class Theatre
    {
        [BsonId]
        public ObjectId TheatreID { get; set; }


        public string TheatreName { get; set; }

        public int Seat { get; set; }




    }
}
