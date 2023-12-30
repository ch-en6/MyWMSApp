using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace wmsApp.pojo
{
    public class IOMaterial
    {
        public long id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string houseName { get; set; }
        public int stock { get; set; }
        public int count { get; set; }
        public string notes { get; set; }
        public IOMaterial() { }

        public IOMaterial(long id, string type, string name, string houseName, int stock, int count, string notes)
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.houseName = houseName;
            this.stock = stock;
            this.count = count;
            this.notes = notes;
        }
    }
}
