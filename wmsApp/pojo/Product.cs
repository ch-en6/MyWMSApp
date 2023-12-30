using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    public class Product
    {
        public string type { get; set; }
        public string name { get; set; }
        public string house { get; set; }
        public long materialId { get; set; }
        public int stock { get; set; }
        public int count { get; set; }
        public string notes { get; set; }
        public Product() { }
    }
}
