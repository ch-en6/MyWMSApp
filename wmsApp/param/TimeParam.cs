using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.param
{
    class TimeParam
    {
        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

        public TimeParam(DateTime? startTime, DateTime? endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
}
