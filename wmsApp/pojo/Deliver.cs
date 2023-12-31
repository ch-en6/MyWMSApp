using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    public class Deliver
    {
        public long deliverId { get; set; }

        public long deliverNo { get; set; }

        public long DeliverNo {  get; set; }

        public string HouseName { get; set; }

        public DateTime DeliverTime { get; set; }

        public long materialId { get; set; }

        public int deliverCount { get; set; }

        public int RemainCount { get; set; }

        public long UserId { get; set; }

        public string notes { get; set; }

        //public int Stock { get; set; }
        public string Category { get; set; }
        public Deliver(long deliverId, string houseName, DateTime deliverTime, long materialId, int deliverCount, long userId, string notes)
        {
            DeliverId = deliverId;
            HouseName = houseName;
            DeliverTime = deliverTime;
            MaterialId = materialId;
            DeliverCount = deliverCount;
            UserId = userId;
            Notes = notes;
        }

        public Deliver(long deliverId, long deliverNo, string houseName, DateTime deliverTime, long materialId, int deliverCount, long userId, string notes)
        {
            this.deliverId = deliverId;
            this.deliverNo = deliverNo;
            this.houseName = houseName;
            this.deliverTime = deliverTime;
            this.materialId = materialId;
            this.deliverCount = deliverCount;
            this.userId = userId;
            this.notes = notes;
        }
    }
}
