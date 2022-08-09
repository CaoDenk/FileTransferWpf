using Avalonia.Threading;

namespace FileTransfer.Tools
{
    internal class MyMessageBox
    {

        public static MessageBoxResult Show(string title, string info)
        {

            Dispatcher.UIThread.Post(() =>
            {
                var msg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(title, info);

                msg.Show();
            });
           
            return MessageBoxResult.OK;
        }
        public static MessageBoxResult Show(string title, string info, MessageBoxButton boxButton)
        {
            //var msg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(title, info);

            //msg.Show();
            return new MessageBoxResult();
        }
    }


    enum MessageBoxResult
    {
        Yes,
        OK,
        CANCEL

    }

    enum MessageBoxButton
    {

        YesNo
    }

}
