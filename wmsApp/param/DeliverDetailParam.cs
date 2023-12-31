using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using wms.pojo;

namespace wmsApp.param
{
    public class DeliverDetailParam
    {
        public int deliverId { get; set; }
        public long deliverNo { get; set; }

        public long materialId { get; set; }

        public string houseName { get; set; }

        public int deliverCount { get; set; }

        public DateTime deliverTime { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

        public Material material { get; set; }
    }
}
