using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class MongoDbSetting
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string BlobConnection { get; set; }
    }
}
