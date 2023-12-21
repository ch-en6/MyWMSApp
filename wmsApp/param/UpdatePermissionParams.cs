using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.param
{
    class UpdatePermissionParams
    {


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
        /**
         * 标志
         */
        public bool? flag { get; set; }

        public UpdatePermissionParams(long userId, long resourceId, string type, bool? flag)
        {
            this.userId = userId;
            this.resourceId = resourceId;
            this.type = type;
            this.flag = flag;
        }
    }
}
