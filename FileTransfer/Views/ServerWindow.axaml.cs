using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FileTransfer.Tools;
using FileTransfer.ViewModels;
using System.ComponentModel;

namespace FileTransfer.Views
{
    public partial class ServerWindow : Window
    {
        ServerWindowViewModel serverWindowViewModel;

        public ServerWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
           
            serverWindowViewModel = new ServerWindowViewModel();
            DataContext = serverWindowViewModel;
        }



        private void BindPort(object sender, RoutedEventArgs e)
        {
            if (serverWindowViewModel.IsBound)
            {
                MyMessageBox.Show("警告","已经绑定，无需重复绑定");
                return;
            }
            bool res = serverWindowViewModel.Bind();
            if (!res)
            {
                MyMessageBox.Show("错误","绑定端口失败，请尝试更换端口");
            }
            else
            {
                Button btn = (Button)(sender);
                btn.Content = "已绑定";
                btn.Background = Brush.Parse("LightBlue");
                serverWindowViewModel.Listen(stackTag,SendText,this);
            }

        }

        private void SendText(object sender, RoutedEventArgs e)
        {
            serverWindowViewModel.SendText((Button)sender);
        }
        private void ReSend(object sender, RoutedEventArgs e)
        {
            serverWindowViewModel.ReSend();
        }
        protected override void OnClosing(CancelEventArgs e)
        {

            serverWindowViewModel.Close();
        }

    }
}
