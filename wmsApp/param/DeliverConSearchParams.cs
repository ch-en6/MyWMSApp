using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.param
{
    public class DeliverConSearchParams
    {
        public long deliverNo { get; set; }

        public string houseName { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

        public long materialId { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

        public int page { get; set; }

        public DeliverConSearchParams() { }

        public DeliverConSearchParams(long deliverNo, string houseName, DateTime? startTime, DateTime? endTime, long materialId, long userId, string notes, int page)
        {
            this.deliverNo = deliverNo;
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
