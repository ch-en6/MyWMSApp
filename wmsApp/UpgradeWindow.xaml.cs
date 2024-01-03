using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wmsApp
{

    public partial class UpgradeWindow : Window
    {
        public UpgradeWindow()
        { 
            Loaded += OnWindowLoaded;
            InitializeComponent();
           
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            WindowStartupLocation = WindowStartupLocation.Manual; // 设置窗口的启动位置为手动模式
        }
        public void UpdateProgress(int percent)
        {
            progressBar.Value = percent;
        }
    }
}
