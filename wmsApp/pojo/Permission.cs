using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.pojo
{
    class Permission
    {
        public Permission(long? id, long userId, long resourceId, string type)
        {
            this.id = id;
            this.userId = userId;
            this.resourceId = resourceId;
            this.type = type;
        }

        public  long? id { get; set; }

        /**
         * 人员id
         */
        public long userId { get; set; }

        /**
         * 资源id
         */
        public long resourceId { get; set; }

        /**
         * 权限类型
         */
        public String type { get; set; }
    }
}
