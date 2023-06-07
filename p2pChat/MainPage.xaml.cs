using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;

namespace p2pchat;

public partial class MainPage : ContentPage
{


	public MainPage()
	{
		InitializeComponent();
        ShowIpv6();
        new Task(Listen).Start();
    }



    //EndPoint remoteIp;
    string localIpv6;
    string localIpv5WithPort;
    Socket socket;
    //List<string> ipList=new();

    /// <summary>
    /// 我觉得应该不需要获取所有公网ipv6，一个设备会有多个公网ipv6，临时ipv6也能够完成通信
    /// 所以只获取一个ipv6 应该也是可行的
    /// </summary>
    /// <returns></returns>
    string GetLocalIpv6()
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface ni in interfaces)
        {

            IPInterfaceProperties ipProps = ni.GetIPProperties();

            foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            {
                if (addr.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (addr.Address.IsIPv6LinkLocal || addr.Address.ToString().StartsWith("::"))
                    {
                        continue;
                    }
                    //var localIp = (IPEndPoint)socket.LocalEndPoint;


                    //ipv6str = ;
                    return  addr.Address.ToString();
                    //ipList.Add(localIpv6);
                   
                }
            }

        }
        return null;
    }
    void ShowIpv6()
    {
        int port = 62580;
        IPEndPoint ep = new IPEndPoint(IPAddress.IPv6Any, port);
        socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ep);

        new Task(async () => { 
            
            while (true)
            {
                string  newLocalIpv6Str=GetLocalIpv6();
                if(newLocalIpv6Str != localIpv6)
                {


                    localIpv6= newLocalIpv6Str;

                    if (newLocalIpv6Str != null)
                    {
                        localIpv5WithPort = $"[{newLocalIpv6Str}]:{port}";
                        Dispatcher.Dispatch(() =>
                        {
                            showLocalIpv6.Text = localIpv5WithPort;
                        });
                    }else
                    {
                        localIpv5WithPort = null;
                        Dispatcher.Dispatch(() =>
                        {
                            showLocalIpv6.Text = "没有ipv6";
                        });
                    }
                   
                }

                await Task.Delay(1500);
            }
        
        
        }).Start();
        

    }

    private void Copy(object sender, EventArgs e)
    {
        //throw new NotImplementedException();
        //DisplayAlert("剪切板", "test", "取消");

        //Button btn = (Button)sender;

        if(localIpv6!=null)
        {
            var clip = Clipboard.Default;
            clip.SetTextAsync(localIpv5WithPort);
            DisplayAlert("剪切板", $"已复制{localIpv5WithPort}到剪切板", "ok");
        }


    }
    /// <summary>
    /// 监听udp，是否要加上回复功能？
    ///
    /// </summary>
    void Listen()
    {

        while (true)
        {
            try
            {

                EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

                byte[] buffer = new byte[1024];
                int len = socket.Receive(buffer);

                Dispatcher.Dispatch(
                        () =>
                        {
                            recvText.Text = Encoding.UTF8.GetString(buffer, 0, len);
                        } );


            }
            catch (Exception ex)
            {
                //待处理
            }



        }

    }
    private void SendIpv6(object sender, EventArgs e)
    {
      
        if (string.IsNullOrEmpty(remoteIp.Text))
        {
            DisplayAlert("错误", "ip不能为空", "确定");
            return;
        }
        try
        {
            var remoteIpAddr = IPEndPoint.Parse(remoteIp.Text);
            byte[] buffer = Encoding.UTF8.GetBytes(content.Text);
            socket.SendTo(buffer, remoteIpAddr);
        }
        catch (FormatException ex)
        {
            
        }

    }

    //void AddTask()
    //{


    //}

    private void ClearIpv6(object sender, EventArgs e) => remoteIp.Text = "";


    private void ShowFriendsEvent(object sender, EventArgs e)
    {
        //ShowFriends showFriends = new ShowFriends();
        //showFriends.
        Application.Current.MainPage.Navigation.PushAsync(new ShowFriends());
    }

    //private void Copy(object sender, EventArgs e)
    //{

    //    var clip= Clipboard.Default;
    //    clip.SetTextAsync("sfdgdgd");
    //    DisplayAlert("剪切板",clip.GetTextAsync().Result,"取消");
    //}
}

