using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.dto
{
    internal class UserPermission
    {
        public UserPermission()
        {
        }

        public UserPermission(int id, string name, string role, List<string> permissionList)
        {
            this.id = id;
            this.name = name;
            this.role = role;
            this.permissionList = permissionList;
        }

      
        /**
        * 人员代码
        */

        public long? id { get; set; }

        /**
         * 姓名
         */
        public String name { get; set; }

        /**
         * 角色
         */
        public String role { get; set; }
        /**
         * 权限列表
         */
        public List<String> permissionList { get; set; }
    }
}
