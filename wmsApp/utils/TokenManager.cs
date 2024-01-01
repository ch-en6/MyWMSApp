using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wms.pojo;

namespace wmsApp.utils
{
    public static class TokenManager
    {
        public static string token { get; set; } = "null";

       /* public static long  userId { get; set; }*/

        public static string javaPublicKey { get; set; }

        public static Dictionary<string, string> csKey { get; set; }

    }
}
