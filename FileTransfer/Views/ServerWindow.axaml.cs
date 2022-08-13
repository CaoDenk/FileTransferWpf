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
                MessageBox.Show("已经绑定，无需重复绑定", "警告");
                return;
            }
            bool res = serverWindowViewModel.Bind();
            if (!res)
            {
                MessageBox.Show("绑定端口失败，请尝试更换端口", "错误");
            }
            else
            {
                Button btn = (Button)(sender);
                btn.Content = "已绑定";
                btn.Background = Brushes.LightBlue;

                //绑定的时候设置缓冲区大小
                serverWindowViewModel.SetBufSize(UnitSizeComboBox.SelectedIndex);

                serverWindowViewModel.Listen(stackTag,this);

                UnitSizeComboBox.IsHitTestVisible = false;
                //UnitSizeComboBox.MouseEnter-=
                PortTextBox.IsReadOnly = true;
                BufSizeTextBox.IsReadOnly = true;

            }

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
