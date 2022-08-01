


using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FileTransfer.Dialogs;
using FileTransfer.ViewModels;
using System.ComponentModel;

namespace FileTransfer.Views
{
    public partial class ClientWindow : Window
    {
        ClientWindowViewModel clientWindowViewModel;
        string[] upfiles;
        public ClientWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            clientWindowViewModel = new ClientWindowViewModel(changeBtnColor);
            clientWindowViewModel.changeText = changeContent;
            DataContext = clientWindowViewModel;
        }

        //private void InitializeComponent()
        //{
        //    AvaloniaXamlLoader.Load(this);
        //}

        private void Connect(object sender, RoutedEventArgs e)
        {
            if (clientWindowViewModel.Connect())
            {
                changeBtnColor(true);
            }

        }
        /// <summary>
        /// 显示内容到TextBlock
        /// </summary>
        /// <param name="s"></param>
        public void changeContent(string s)
        {

            Dispatcher.UIThread.Post(() =>
            {
                ShowFile.Text = s;

            });
        }

        public void changeBtnColor(bool connected)
        {
            if (connected)
            {
                Dispatcher.UIThread.Post(() => {
                    Btn.Background = Brush.Parse("LightBlue");
                    Btn.Content = "已连接";
                });

            }
            else
            {
                Dispatcher.UIThread.Post(() => {
                    Btn.Background = Brush.Parse("Red");
                    Btn.Content = "未连接";

                });
            }

        }
        public void ChooseFiles(object sender,RoutedEventArgs e)
        {
            ChooseFiles choose = new();
            upfiles =  choose.open(this);
            if (upfiles == null)
                return;
            string s = "";
            foreach (string s2 in upfiles)
            {
                s += s2 + "\r\n";
            }
            ShowFile.Text=s;
        }
        private void SendText(object sender, RoutedEventArgs e)
        {
            clientWindowViewModel.sendText(Content.Text);
        }
        private void SendFiles(object sender, RoutedEventArgs e)
        {
            clientWindowViewModel.sendFile(upfiles);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            clientWindowViewModel.CloseSocket();
        }
    }
}
