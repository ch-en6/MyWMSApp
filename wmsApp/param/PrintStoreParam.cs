using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using wms.pojo;

namespace wmsApp.param
{
    public class PrintStoreParam
    {
        public long storeNo { get; set; }

        public string materialName { get; set; }

        public long materialId { get; set; }

        public string materialType { get; set; }

        public string houseName { get; set; }

        public int storeCount { get; set; }

        public string materialUnit { get; set; }

        public string storeTime { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }
    }
}
