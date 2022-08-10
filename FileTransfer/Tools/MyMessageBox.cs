using Avalonia.Threading;
using FileTransfer.Elements;
using MessageBox.Avalonia.Enums;

namespace FileTransfer.Tools
{
    internal class MyMessageBox
    {

        public static  ButtonResult Show(string title, string info, ButtonEnum buttons =default)
        {

            Dispatcher.UIThread.InvokeAsync(  () =>
            {
                var msg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(title, info,buttons);

                  msg.Show();
            });

            return ButtonResult.Yes;
        }
    
    }


 

}
