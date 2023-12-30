using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    public class Store
    {
        public long storeId { get; set; }

        public long storeNo { get; set; }

        public string houseName { get; set; }

        public DateTime storeTime { get; set; }

        public long materialId { get; set; }

        public int storeCount { get; set; }

        public int remainCount { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

        public Store() { }

        public Store(long storeId, long storeNo, string houseName, DateTime storeTime, long materialId, int storeCount, long userId, string notes)
        {
            this.storeId = storeId;
            this.storeNo = storeNo;
            this.houseName = houseName;
            this.storeTime = storeTime;
            this.materialId = materialId;
            this.storeCount = storeCount;
            this.userId = userId;
            this.notes = notes;
        }
    }
}
