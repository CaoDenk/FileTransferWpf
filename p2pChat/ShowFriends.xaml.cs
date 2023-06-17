using CommunityToolkit.Maui.Views;
using Microsoft.Data.Sqlite;
using p2pchat.Crud;
using p2pchat.Init;
using p2pchat.pojo;
using p2pchat.Settings;
using p2pchat.UdpCommunication;
using p2pchat.Views;
using System.Diagnostics;
using System.Text;


namespace p2pchat;

public partial class ShowFriends : ContentPage
{
    Dictionary<string,User> userDict = new Dictionary<string,User>();
	public ShowFriends()
	{
		InitializeComponent();
        ListAllFriends();
        tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
      
        swipeGestureRecognizer.Direction = SwipeDirection.Left;
        swipeGestureRecognizer.Swiped += DelFriend;
    }

    private  void DelFriend(object sender, SwipedEventArgs e)
    {
        Label label = (Label)sender;
        FriendsDao.DeleteFriendByUid(label.ClassId);
        LabelsStack.Remove((VerticalStackLayout)label.Parent);

        userDict.Remove(label.ClassId);



    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    ListAllFriends();
    //}
    List<User> friends;
    public  async void ListAllFriends()
	{
        //LabelsStack.Children.Clear();
        //userDict.Clear();
        friends = await FriendsDao.GetFriendsFromSqlite();

        foreach (User user in friends)
        {
            Label label = new Label();


            //gestureRecognizer.PropertyChanged.Add(label);
            label.Text = user.username;
            label.FontSize = 24;
            label.ClassId = user.uid;
            label.GestureRecognizers.Add(tapGestureRecognizer);
            label.GestureRecognizers.Add(swipeGestureRecognizer);

            VerticalStackLayout vert=new VerticalStackLayout();
            vert.Margin= new Thickness(2, 2, 2, 2);
            vert.HeightRequest = 40;
            vert.Add(label);
            vert.VerticalOptions=LayoutOptions.Center;
            vert.Background = Brush.AliceBlue;


            LabelsStack.Add(vert);
        
            userDict.Add(user.uid, user);
        }

	}

    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
   
    SwipeGestureRecognizer swipeGestureRecognizer=new SwipeGestureRecognizer();

    /// <summary>
    /// 点击好友进入聊天框
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
       Label label=(Label)sender;

       //User u= friends.Where(e => e.uid == label.ClassId).First();
        User u = userDict[label.ClassId];
        ChatWithFriend chatWithFriend = new ChatWithFriend();
        DisplayRecvMsg.chatDialog = chatWithFriend;

        Global.GlobalVar.uidChatWith = u.uid;
        chatWithFriend.Title = label.Text;
        Navigation.PushAsync(chatWithFriend);
    }

    private async void Add(object sender, EventArgs e)
    {
        //var popup = new Popup();
        ////popup.Content = 
        string action = await DisplayActionSheet("添加选项", "取消", null, "添加好友", "通过Ipv6对话");

        switch (action)
        {
            case "添加好友":
                Application.Current.MainPage.Navigation.PushAsync(new Add());
                break;
            case "通过Ipv6对话":
                //Shell.Current.GoToAsync(nameof(ChatDialogDirectly),true);
                ChatDialogDirectly chatDialogDirectly = new ChatDialogDirectly();
                DisplayRecvMsg.chatDialog=chatDialogDirectly;
                Application.Current.MainPage.Navigation.PushAsync(chatDialogDirectly);
                break;
        }

        //Debug.WriteLine("Action: " + action);
    }
}