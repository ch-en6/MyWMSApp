using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using wms.pojo;

namespace wmsApp.param
{
    public class StoreDetailParam
    {
        public int storeId { get; set; }
        public long storeNo { get; set; }

        public long materialId { get; set; }

        public string houseName { get; set; }

        public int storeCount { get; set; }

        public int remainCount { get; set; }

        public DateTime storeTime { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

        public Material material { get; set; }
    }
}
