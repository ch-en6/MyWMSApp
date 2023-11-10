using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.dto
{
    class Result
    {
        public Result()
        {
        }

        public Result(bool success, string errorMsg, object data, long total)
        {
            this.success = success;
            this.errorMsg = errorMsg;
            this.data = data;
            this.total = total;
        }

        public Result(bool success, string errorMsg, int code, object data, long total)
        {
            this.success = success;
            this.errorMsg = errorMsg;
            this.code = code;
            this.data = data;
            this.total = total;
        }

        public Boolean success { get; set; }
        public String errorMsg { get; set; }
        public int code { get; set; } = -1;
        public Object data { get; set; }
        public long total { get; set; }
    }
}
