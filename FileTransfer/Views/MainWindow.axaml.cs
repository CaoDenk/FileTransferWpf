using Avalonia.Controls;
using Avalonia.Interactivity;
using FileTransfer.Tools;
using FileTransfer.ViewModels;

namespace FileTransfer.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;
           
        }

        private void navigateToClient(object sender, RoutedEventArgs e)
        {
            ClientWindow client = new();
            client.Show();
        }

        private void navigateToServer(object sender, RoutedEventArgs e)
        {
            //MyMessage.show("≤‚ ‘"," «∑Ò¬“¬Î");
            //if(ServerBtn==null)
            //{

            //}
            //if(server.ISC)
            

            ServerWindow server = new ServerWindow();
            server.Show();

        }
    }
}
