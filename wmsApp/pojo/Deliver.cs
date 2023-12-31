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

        public string houseName { get; set; }

        public DateTime deliverTime { get; set; }

        public long materialId { get; set; }

        public int deliverCount { get; set; }

        public long userId { get; set; }

        public string notes { get; set; }

        public Deliver() { }

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