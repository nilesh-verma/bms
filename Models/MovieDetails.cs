using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class MovieDetails
    {
        public string TID { get; set; }

        public string MovieName { get; set; }
        public string Actor { get; set; }
        public string length { get; set; }
        public string imageUrl { get; set; }
        public string Description { get; set; }

        public int seat { get; set; }
    }
}
