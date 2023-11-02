using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.param
{
    class UpdatePermissionParams
    {
        public UpdatePermissionParams(long userId, long resourceId, string type)
        {
            this.userId = userId;
            this.resourceId = resourceId;
            this.type = type;
        }


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
