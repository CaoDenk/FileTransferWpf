using p2pchat.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.UdpCommunication
{
    internal class DisplayRecvMsg
    {

        public static ChatDialog chatDialog;

        public static void Show(string msg)
        {
            Application.Current.Dispatcher.Dispatch(() =>chatDialog?.ShowMsg(msg, Color.FromRgb(0, 0, 255), LayoutOptions.Start));          
            //if(App.Current.MainPage)
        }


    }
}
