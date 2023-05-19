using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FileTransfer.ViewModels;

namespace FileTransfer.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }



        private void NavicateToClientWindow(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWindow = new ClientWindow();
            clientWindow.Show();


        }
        private void NavicateToServerWindow(object sender, RoutedEventArgs e)
        {
            ServerWindow serverWindow = new ServerWindow();
            serverWindow.Show();
        }

    }
}
