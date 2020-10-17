using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class UserDetails
    {
        [BsonId]
        public ObjectId UserId { get; set; }

        public string uid { get; set; }

        public string TID { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public int Seat { get; set; }

    }
}
