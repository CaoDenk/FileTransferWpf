
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS
using Windows.UI.Popups;
#endif
namespace p2pchat.Utils
{
    internal class MsgPush
    {
        static int notificationId = 101;

        public static void Push(string title,string desp)
        {


#if ANDROID
            var request = new NotificationRequest
            {
                NotificationId = ++notificationId,
                Title = title,
                Description = desp,
                ReturningData = "Dummy data",
                //NotifyTime = DateTime.Now.AddSeconds(10) // You can set the notify time
            };

            // Show the notification
            LocalNotificationCenter.Current.Show(request);

#elif WINDOWS
          
            MessageDialog message = new MessageDialog(desp, title);
            message.ShowAsync();
#endif
        }

    }
}
