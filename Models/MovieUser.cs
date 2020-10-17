using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class MovieUser
    {
        public MovieDetails movieDetails { get; set; }
        public UserDetails userDetails { get; set; }
    }
}
