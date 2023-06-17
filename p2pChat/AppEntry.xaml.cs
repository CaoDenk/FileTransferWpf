using CommunityToolkit.Maui.Alerts;
using p2pchat.Code;
using p2pchat.Crud;
using p2pchat.CrudRequest;
using p2pchat.ExeTask;
using p2pchat.Global;

namespace p2pchat;

public partial class AppEntry : Shell
{
	public AppEntry()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AppShell), typeof(AppShell));
    }

    private async void InvokeBrowser(object sender, EventArgs e)
    {
        Uri uri = new Uri(RequestApi.REGISTE_URL);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

    }

    private async void Login(object sender, EventArgs e)
    {
        string zone = Zone.Text;
        string name=Name.Text;
        string password=Password.Text;

        Login login = new Login(name,password,zone);

        LoginResp resp= await login.ToLogin();
        
        if(resp.statusCode==(int)StatusCode.SUCCESS)
        {

            ListenTask.Init();
            GlobalVar.uuid = resp.uuid;
            App.Current.MainPage = new AppShell();

        }else
        {
            DisplayAlert("¥ÌŒÛ", "√‹¬Î¥ÌŒÛ", "»∑∂®");
        }
      
    }
}