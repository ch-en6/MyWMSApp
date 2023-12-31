using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.param
{
    public class PrintDeliverParam
    {
        public long deliverNo { get; set; }

        public string materialName { get; set; }

        public long materialId { get; set; }

        public string materialType { get; set; }

        public string houseName { get; set; }

        public int deliverCount { get; set; }

        public string materialUnit { get; set; }

        public string deliverTime { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

    }
}
