using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FileTransfer.Dialogs;
using FileTransfer.Models;
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
            serverWindowViewModel = new ServerWindowViewModel(stackTag);
            DataContext = serverWindowViewModel;

        }

        //private void InitializeComponent()
        //{
        //    AvaloniaXamlLoader.Load(this);
        //}

        //public void changeText(string s)
        //{

        //    Dispatcher.UIThread.Post(() => {
        //        showfile.Text = s;

        //    });
        //}

        public void changeBtnColor(bool connected)
        {
            //if (connected)
            //{
            //    Dispatcher.UIThread.Post(() => {
            //        Btn.Foreground = Brush.Parse("LightBlue");
            //        Btn.Text = "已连接";
            //    });

            //}
            //else
            //{
            //    Dispatcher.UIThread.Post(() => {
            //        Btn.Foreground = Brush.Parse("Red");
            //        Btn.Text = "未连接";

            //    });

            //}
            
        }

        private void Bind(object sender, RoutedEventArgs e)
        {
          

            if (!serverWindowViewModel.IsBound)
            {
                //Button btn = (Button)sender;
                serverWindowViewModel.Bind();
                BindBtn.Background = Brush.Parse("LightBlue");
                BindBtn.Content = "已绑定";
            }
            else
                MyMessage.show("warning", "socket has been bound!");
        }
        //private void ChooseFiles(object sender,RoutedEventArgs e)
        //{

        //    ChooseFiles choose = new();
        //    string[] res = choose.open(this);
        //    if (res == null)
        //        return;
        //    string s = "";
        //    foreach (string s2 in res)
        //    {
        //        s += s2 + "\r\n";
        //    }
        //    //showfile.Text = s;

        //}

       protected override  void OnClosing(CancelEventArgs e)
        {
            serverWindowViewModel.CloseSocket();
        }

    }
}
