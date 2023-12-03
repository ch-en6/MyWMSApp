using ModernWpf.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp1.dto;
using wms;
using wms.pojo;
using wms.utils;
using wmsApp.controls;
using wmsApp.dialog;
namespace wmsApp.pages
{
    /// <summary>
    /// UserPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserPage : System.Windows.Controls.Page
    {
        public UserPage()
        {
            InitializeComponent();
            Result result = UserInfoApi.show();
            if (!result.success)
            {
                ModernMessageBox.showMessage(result.errorMsg);
            }
            User user =JsonHelper.JSONToObject<User>(result.data.ToString());
            DataContext = user;


        }
    }
}
