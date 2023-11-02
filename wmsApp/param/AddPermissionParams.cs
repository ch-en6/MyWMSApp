using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.param
{
    class AddPermissionParams
    {
        public AddPermissionParams(List<long> userIds, long resourceId, string type)
        {
            this.userIds = userIds;
            this.resourceId = resourceId;
            this.type = type;
        }


        /**
         * 人员ids
         */
        public List<long> userIds { get; set; }

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
