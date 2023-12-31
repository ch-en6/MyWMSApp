using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    public class Deliver
    {

        public long DeliverId { get; set; }

        public long DeliverNo {  get; set; }

        public string HouseName { get; set; }

        public DateTime DeliverTime { get; set; }

        public long MaterialId { get; set; }

        public int DeliverCount { get; set; }

        public int RemainCount { get; set; }

        public long UserId { get; set; }

        public string Notes { get; set; }

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

        public Deliver()
        {
        }
    }
}
