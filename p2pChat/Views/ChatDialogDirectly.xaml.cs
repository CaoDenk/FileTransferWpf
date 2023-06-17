using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net;
using System.Text;
using p2pchat.ExeTask;
using p2pchat.UdpCommunication;
using CommunityToolkit.Maui.Alerts;

namespace p2pchat.Views;

public partial class ChatDialogDirectly : ContentPage,ChatDialog
{


	public ChatDialogDirectly()
	{
		InitializeComponent();
      
    }

    private void ClearIpv6(object sender, EventArgs e) => RemoteIpv6.Text = "";
    private void SendMsg(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (string.IsNullOrEmpty(RemoteIpv6.Text))
        {

            button.DisplaySnackbar("对方ipv6不能为空");
            return;
        }

        string msg = InputMsg.Text;
        if (string.IsNullOrEmpty(msg))
        {
          
            button.DisplaySnackbar("消息不能为空");
            return;
        }
        IPEndPoint remoteIpAddr = null;
        try
        {
            remoteIpAddr =  IPEndPoint.Parse(RemoteIpv6.Text);
        }catch (Exception ex)
        {
            return;
        }


        ShowMsg(msg, Color.FromRgb(0, 255, 0), LayoutOptions.End);//显示发送的消息
        
        SendMsgHanle sendMsgHanle=new SendMsgHanle(msg, remoteIpAddr);
        Task.Run(sendMsgHanle.Send);

    }

    
    public  void ShowMsg(string msg, Color color,LayoutOptions options)
    {
        Label label = new Label();
        label.Text = msg;
        label.FontSize = 24; ;
        label.Margin = new Thickness(0, 2, 2, 2);
        label.BackgroundColor =color;
        label.HorizontalOptions = options;
        InputMsg.Text = "";
        ShowMsgStack.Add(label);

    }




}