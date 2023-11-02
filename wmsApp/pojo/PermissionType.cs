using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    class PermissionType
    {

        public long id;

        /**
         * 
         */
        public long resourceId;

        /**
         * 
         */
        public String type;

        /**
         * 
         */
        public String uri;

        public PermissionType(long id, long resourceId, string type, string uri)
        {
            this.id = id;
            this.resourceId = resourceId;
            this.type = type;
            this.uri = uri;
        }
    }
}
