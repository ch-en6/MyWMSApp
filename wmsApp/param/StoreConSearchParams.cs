using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.param
{
    public class StoreConSearchParams
    {
        public long storeNo { get; set; }

        public string houseName { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

        public long materialId { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

        public int page {  get; set; }

        public StoreConSearchParams() { }

        public StoreConSearchParams(long storeNo, string houseName, DateTime? startTime, DateTime? endTime, long materialId, long userId, string notes, int page)
        {
            this.storeNo = storeNo;
            this.houseName = houseName;
            this.startTime = startTime;
            this.endTime = endTime;
            this.materialId = materialId;
            this.userId = userId;
            this.notes = notes;
            this.page = page;
        }
    }
}
