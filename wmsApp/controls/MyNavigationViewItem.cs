using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wmsApp
{
   

        public class MyNavigationViewItem : NavigationViewItem
        {
            public static readonly DependencyProperty UriProperty = DependencyProperty.Register(
                "Uri", typeof(string), typeof(MyNavigationViewItem), new PropertyMetadata(null));

            public string Uri
            {
                get { return (string)GetValue(UriProperty); }
                set { SetValue(UriProperty, value); }
            }
        }

}
