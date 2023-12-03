using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.dto
{
    public class Result
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

        public Result(bool success, string errorMsg, int code, object data, long total,String aesKey,String publicKey)
        {
            this.success = success;
            this.errorMsg = errorMsg;
            this.code = code;
            this.data = data;
            this.total = total;
            this.aesKey = aesKey;
            this.publicKey=publicKey;
        }

        public Result(bool success, string errorMsg, int code, object data, long total, String aesKey)
        {
            this.success = success;
            this.errorMsg = errorMsg;
            this.code = code;
            this.data = data;
            this.total = total;
            this.aesKey = aesKey;
         
        }

        public Boolean success { get; set; }
        public String errorMsg { get; set; }
        public int code { get; set; } = -1;
        public Object data { get; set; }
        public long total { get; set; }
        public String aesKey { get; set; }
        public String publicKey { get; set; }
    }
}
