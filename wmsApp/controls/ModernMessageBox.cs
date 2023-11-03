using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernWpf.Controls;
namespace wmsApp.controls
{


    public static class ModernMessageBox
    {
        public static async Task Show(string title, string message)
        {
            var messageDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "OK"
            };

            await messageDialog.ShowAsync();
        }
    }

}