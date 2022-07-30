using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using 
namespace FileTransfer.Tools
{
    internal class MyMessage
    {

        public static void show(string title,string info)
        {
            var msg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(title, info);

            msg.Show();

        }

    }
}
