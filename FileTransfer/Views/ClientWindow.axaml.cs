


using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FileTransfer.ViewModels;
using System.ComponentModel;
using FileTransfer.Tools;
namespace FileTransfer.Views
{
    public partial class ClientWindow : Window
    {
        ClientWindowViewModel clientWindowViewModel;
        string[] fullFilePaths;
        public ClientWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            clientWindowViewModel = new ClientWindowViewModel();
            clientWindowViewModel.panel=ClientStackTag;
            DataContext = clientWindowViewModel;
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            if (clientWindowViewModel.IsConnected)
            {
                MessageBox.Show("已经连接，请勿重复操作");
                return;
            }
            clientWindowViewModel.SetBufSize(UnitSizeComboBox.SelectedIndex);
            clientWindowViewModel.Connect(ChangeBtnColor);
        }

        private void SendText(object sender, RoutedEventArgs e)
        {
            clientWindowViewModel.SendText();
        }
        private async void OpenFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Title = "一次只能最多选两个，否则一次也只发送前两个";
            //openFileDialog.Multiselect = true;
            var res=  await  openFileDialog.ShowAsync(this);
            //var res = openFileDialog.ShowDialog();
            if (res!=null)
            {

                fullFilePaths = res;
                foreach (string s in fullFilePaths)
                {
                    string tmp = s;
                    tmp += "\r\n";
                    clientWindowViewModel.ShowContent += tmp;

                }
            }

        }

        private void SendFile(object sender, RoutedEventArgs e)
        {
            if (fullFilePaths != null)
                clientWindowViewModel.SendFileRequest(fullFilePaths);
            else
                MessageBox.Show("您还未选择文件,请选择文件");
        }
        void ChangeBtnColor(bool connected)
        {
            if (connected)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ConnBtn.Content = "已连接";
                    ConnBtn.Background = Brushes.LightBlue;

                    UnitSizeComboBox.IsHitTestVisible = false;
                    IpTextBox.IsReadOnly = true;
                    PortTextBox.IsReadOnly = true;
                    BufSizeTextBox.IsReadOnly = true;
                });
            }
            else
            {
                
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ConnBtn.Content = "未连接";
                    ConnBtn.Background = Brushes.Red;
                    UnitSizeComboBox.IsHitTestVisible = true;
                    IpTextBox.IsReadOnly = false;
                    PortTextBox.IsReadOnly = false;
                    BufSizeTextBox.IsReadOnly = false;
                });

            }

        }
        protected override void OnClosing(CancelEventArgs e)
        {

            clientWindowViewModel.Close();
        }

        private void ClearSendFileList(object sender, RoutedEventArgs e)
        {
            fullFilePaths = null;
            clientWindowViewModel.ShowContent = "";
        }
    }
}
