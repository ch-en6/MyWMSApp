using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    public class Store
    {
        public long StoreId { get; set; }

        public long StoreNo { get; set; }

        public string HouseName { get; set; }

        public DateTime StoreTime { get; set; }

        public long MaterialId { get; set; }

        public int StoreCount { get; set; }

        public long UserId { get; set; }

        public string Notes { get; set; }

        public Store() { } 

        public Store(long storeId, long storeNo, string houseName, DateTime storeTime, long materialId, int storeCount, long userId, string notes)
        {
            StoreId = storeId;
            StoreNo = storeNo;
            HouseName = houseName;
            StoreTime = storeTime;
            MaterialId = materialId;
            StoreCount = storeCount;
            UserId = userId;
            Notes = notes;
        }
    }
}
