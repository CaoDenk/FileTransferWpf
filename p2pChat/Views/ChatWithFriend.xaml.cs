using CommunityToolkit.Maui.Alerts;
using p2pchat.Code;
using p2pchat.ExeTask;
using p2pchat.Global;
using p2pchat.HttpRequest;
using p2pchat.pojo;
using p2pchat.UdpCommunication;
using System.Net;
using System.Text;

namespace p2pchat.Views;

public partial class ChatWithFriend : ContentPage,ChatDialog
{
    //public User user { get; set; }
    public ChatWithFriend()
	{
		InitializeComponent();

	}
    private async void SendMsg(object sender, EventArgs e)
    {

        string msg = InputMsg.Text;
        if(string.IsNullOrEmpty(msg))
        {
            Button button=sender as Button;
            button.DisplaySnackbar("消息不能为空");
            return;
        }

        ShowMsg(msg, Color.FromRgb(0, 255, 0), LayoutOptions.End);//先显示发送的消息
        try
        {
            GetRessult getRessult= await GetIpv6ByUid.GetIpv6(GlobalVar.uidChatWith);
            if(getRessult.statusCode==(int)StatusCode.SUCCESS)
            {
                IPEndPoint remoteIpAddr = IPEndPoint.Parse(getRessult.ipv6);

                SendMsgHanle sendMsgHanle = new SendMsgHanle(msg, remoteIpAddr);
                Task.Run(sendMsgHanle.Send);
            }

        }
        catch (FormatException ex)
        {

        }

    }
    public void ShowMsg(string msg, Color color, LayoutOptions options)
    {
        Label label = new Label();
        label.Text = msg;
        label.FontSize = 24; ;
        label.Margin = new Thickness(0, 2, 2, 2);
        label.BackgroundColor = color;
        label.HorizontalOptions = options;
        InputMsg.Text = "";
        ShowMsgStack.Add(label);

    }
    protected override void OnDisappearing()
    {
        DisplayRecvMsg.chatDialog = null;
    }

}