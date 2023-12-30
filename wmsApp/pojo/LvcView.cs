using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace wms.pojo
{
    public class LvcView
    {
        //int[] ints = new int[] { 53, 60, 36, 52 };
        //public string[] labels = { "物品","哈哈","嗯嗯","嘿嘿"};

        public ChartValues<int> ChartDatas { get; set; }
        public List<string> XLables { get; set; }
        //public LvcView()
        //{
        //    for(int i=0;i<ints.Length;i++)
        //    {
        //        ChartDatas.Add(ints[i]);
        //    }
        //    for(int i=0;i<labels.Length;i++)
        //    {
        //        XLabels.Add(labels[i]);
        //    }
        //}

    }
}
