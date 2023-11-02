using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.pojo
{
    public class User
    {
        /**
 * 人员代码
 */
        public long id;

        /**
         * 密码
         */
        public String password;

        /**
         * 姓名
         */
        public String name;

        /**
         * 角色
         */
        public String role;

        /**
         * 性别
         */
        public String sex;

        /**
         * 出生日期
         */
        public DateTime birthDate;

        /**
         * 身份证号
         */
        public String idNumber;

        /**
         * 籍贯
         */
        public  String nativePlace;

        /**
         * 家庭住址
         */
        public String address;

        /**
         * 联系电话
         */
        public String phone;
    }
}
