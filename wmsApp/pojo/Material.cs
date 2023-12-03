using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.pojo
{
 


    public class Material
    {
        public long id { get; set; }

        /**
         * 物料名称
         */
        public String name { get; set; }

        /**
         * 仓库代码
         */
        public string houseName { get; set; }

        /**
         * 规格型号
         */
        public String type { get; set; }

        /**
         * 计量单位
         */
        public String unit { get; set; }

        /**
         * 库存数量
         */
        public int stock { get; set; }

        /**
         * 备注
         */
        public String comments { get; set; }

        /**
         * 创建时间
         */
       
        public DateTime createTime { get; set; }

        public Material()
        {
           
        }
        public Material(long id, string name, string houseName, string type, string unit, int stock, string comments, DateTime createTime)
        {
            this.id = id;
            this.name = name;
            this.houseName = houseName;
            this.type = type;
            this.unit = unit;
            this.stock = stock;
            this.comments = comments;
            this.createTime = createTime;
        }

    }
}
