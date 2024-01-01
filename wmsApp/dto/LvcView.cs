using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace wms.dto
{
    public class LvcView
    {
        public ChartValues<int> ChartDatas { get; set; }
        public List<string> XLables { get; set; }
        
    }
}
