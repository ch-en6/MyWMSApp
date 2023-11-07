using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wmsApp.pojo
{
    class Resource
    {
        /**
    * 资源代码
    */

        public long id { get; set; }

        /**
         * 资源名称
         */
        public String name { get; set; }
        /**
         * uri名称
         */
        public String uriName { get; set; }
        /**
         * icon
         */
        public String icon { get; set; }
        /**
         * 页面
         */
        public String page { get; set; }

        public Resource()
        {
        }
        public override string ToString()
        {
            return $"Resource{{id={id}, name='{name}', uriName='{uriName}', icon='{icon}', page='{page}'}}";
        }
        public Resource(long id, string name, string uriName, string icon, string page)
        {
            this.id = id;
            this.name = name;
            this.uriName = uriName;
            this.icon = icon;
            this.page = page;
        }
    }
}
