using p2pchat.Crud;
using p2pchat.ExeTask;
using p2pchat.Global;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;



namespace p2pchat.Settings;

public partial class AppSettings 
{
	public AppSettings()
	{
		InitializeComponent();
        ptr = DownProgress;
        if (Percentage!=0)
        {
            DownProgress.Progress=Percentage/100d;
            DownProgress.IsVisible=true;
            
        }
        
     
    }


    static ProgressBar ptr;
    static int percentage;
    static int Percentage 
    {
        get { return percentage; }
        set
        {
            percentage = value;

            ptr.Progress = percentage/100d ;

        }
    }


    /// <summary>
    /// WebClient 已经过时，有时间在修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Update(object sender, EventArgs e)
    {

        //HttpClient httpClient = new HttpClient();
       
        using (WebClient client = new WebClient())
        {     
            string apkPath = $"{Config.DOWNLOAD_PATH}/quora.apk";
            DownProgress.IsVisible = true;
            client.DownloadFileCompleted += async (sender, e) =>
            {
                ptr.IsVisible = false;
                Percentage = 0;
                PromptInstall(apkPath);
            };
 
            client.DownloadProgressChanged += (sender, e) =>
            {
                Percentage = e.ProgressPercentage;
            };
            client.DownloadFileAsync(new Uri(RequestApi.UPDATE_URL), apkPath);
         
        }



        async void PromptInstall(string apkPath)
         {


            var alert = await DisplayAlert("提示", "是否安装", "确定", "取消");
            if (alert)
            {
                await Launcher.Default.OpenAsync(new OpenFileRequest("Install app", new ReadOnlyFile(apkPath)));
            }

        }

    }

    private async void About(object sender, EventArgs e)
    {
        Uri uri = new Uri(RequestApi.ABOUT_URL);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    private void LogOut(object sender, EventArgs e)
    {
        App.Current.MainPage=new AppEntry();
    }

    private async void ShowSelfIpv6(object sender, EventArgs e)
    {

        string ipv6WithPort = Utils.Utils.GetIpv6WithPortStr();    
        var alert = await DisplayAlert("提示", $"{(ipv6WithPort ?? "没有ipv6")},是否复制ipv6", "复制", "取消");
        if (alert)
        {
            var clip = Clipboard.Default;
            clip.SetTextAsync(ipv6WithPort);
        }
    }


}