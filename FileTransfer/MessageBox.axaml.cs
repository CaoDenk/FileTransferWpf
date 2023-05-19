using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace FileTransfer
{
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }


        
        public static Task<MessageBoxResult> Show(string text, string title = "消息", MessageBoxButtons buttons = default, Window parent = null)
        {
            return Dispatcher.UIThread.InvokeAsync<MessageBoxResult>(() => {

                
                var msgbox = new MessageBox()
                {
                    Title = title,
                    Width = text.Length * 14+ 110,
                    Height = 100,
                    

                };
                msgbox.FindControl<TextBlock>("Text").Text = text;
                var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

                //buttonPanel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
                var res = MessageBoxResult.Ok;

                void AddButton(string caption, MessageBoxResult r, bool def = false)
                {
                    var btn = new Button { Content = caption };

                    btn.Click += (_, __) =>
                    {
                        res = r;
                        msgbox.Close();
                    };
                    buttonPanel.Children.Add(btn);
                    if (def)
                        res = r;
                }

                if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
                    AddButton("Ok", MessageBoxResult.Ok, true);
                if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
                {
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No, true);
                }

                if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
                    AddButton("Cancel", MessageBoxResult.Cancel, true);


                var tcs = new TaskCompletionSource<MessageBoxResult>();
                msgbox.Closed += delegate { tcs.TrySetResult(res); };
                if (parent != null)
                    msgbox.ShowDialog(parent);
                else msgbox.Show();
                return tcs.Task;
            }
            );
        }
    }
}
