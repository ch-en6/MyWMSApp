using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.param
{
    class LoginParams
{
        public long userId;
        public string password;

        public LoginParams(long userId, string password)
        {
            this.userId = userId;
            this.password = password;
        }
    }
}
