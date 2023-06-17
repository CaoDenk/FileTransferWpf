using p2pchat.Init;
using p2pchat.Views;

namespace p2pchat;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        DbOp.Init();

        MainPage = new AppEntry();
        //MainPage = new AppShell();


    }
}
