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
        public long id { get; set; }

        /**
         * 密码
         */
        public String password { get; set; }

        /**
         * 姓名
         */
        public String name { get; set; }

        /**
         * 角色
         */
        public String role { get; set; }

        /**
         * 性别
         */
        public String sex { get; set; }

        /**
         * 出生日期
         */
        public DateTime birthDate { get; set; }

        /**
         * 身份证号
         */
        public String idNumber { get; set; }

        /**
         * 籍贯
         */
        public String nativePlace { get; set; }

        /**
         * 家庭住址
         */
        public String address { get; set; }

        /**
         * 联系电话
         */
        public String phone { get; set; }

        public User(string name, string role, string sex, DateTime birthDate, string idNumber, string nativePlace, string address, string phone)
        {
            this.name = name;
            this.role = role;
            this.sex = sex;
            this.birthDate = birthDate;
            this.idNumber = idNumber;
            this.nativePlace = nativePlace;
            this.address = address;
            this.phone = phone;
        }

        public User()
        {
        }

        public User(string name, string role, string sex, string idNumber, string nativePlace, string address, string phone)
        {
            this.name = name;
            this.role = role;
            this.sex = sex;
            this.idNumber = idNumber;
            this.nativePlace = nativePlace;
            this.address = address;
            this.phone = phone;
        }

        public User(long id, string password, string name, string role, string sex, DateTime birthDate, string idNumber, string nativePlace, string address, string phone)
        {
            this.id = id;
            this.password = password;
            this.name = name;
            this.role = role;
            this.sex = sex;
            this.birthDate = birthDate;
            this.idNumber = idNumber;
            this.nativePlace = nativePlace;
            this.address = address;
            this.phone = phone;
        }


    }


}
