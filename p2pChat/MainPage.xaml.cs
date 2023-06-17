using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;
using p2pchat.CrudRequest;
using p2pchat.Settings;
using p2pchat.Global;
using p2pchat.ExeTask;

namespace p2pchat;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

    }

    

    private void NavigateToSettings(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PushAsync(new AppSettings());
    }


}

