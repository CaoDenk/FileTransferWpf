using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using p2pchat.Code;
using p2pchat.Crud;
using p2pchat.HttpRequest;


namespace p2pchat.Settings;

public partial class Add : ContentPage
{
	public Add()
	{
		InitializeComponent();
	}
	Dictionary<string, QueryUserResult> SaveUsrDict = new Dictionary<string, QueryUserResult>();
    private async void SearchUser(object sender, EventArgs e)
    {
		QueryUser queryUser = new QueryUser();
		QueryUserResult result= await queryUser.Query(UserName.Text);
		if(result.statusCode==(int)StatusCode.SUCCESS)
		{

			Grid grid=new Grid();
			grid.Margin= new Thickness(2, 2, 2, 2);
            GridLength gridWidth=new GridLength(1,GridUnitType.Star);
			ColumnDefinition[] coldfs=new ColumnDefinition[2];
            for (int i = 0; i < coldfs.Length; i++)
            {
                coldfs[i] = new ColumnDefinition(gridWidth);
            }

            grid.ColumnDefinitions = new ColumnDefinitionCollection(coldfs);

            Label label = new Label
            {
                Text = result.username,
                
				VerticalTextAlignment=TextAlignment.Center
            };
            Button button = new Button
            {
                Text = "保存到好友列表",
                ClassId=result.uid,
				HorizontalOptions=LayoutOptions.End
            };
            button.Clicked += SaveUser;

			SaveUsrDict.Add(result.uid, result);

			grid.Add(label,row:0,column:0);
			grid.Add(button,row:0,column:1);

			SearchResultStack.Add(grid);
			
        }


    }

    private void SaveUser(object sender, EventArgs e)
    {
		Button button = (Button)sender;

		QueryUserResult queryUser = SaveUsrDict[button.ClassId];

		//GetFriends getFriends=new GetFriends();
		if(FriendsDao.Exists(queryUser.uid))
		{
            Toast.Make("已添加到好友列表").Show();
            //button.DisplaySnackbar("已添加到好友列表");
        }
        else
		{
            FriendsDao.InsertUser(queryUser);

            //DisplayAlert("提示", "已添加到好友列表", "确定");
            //ISnackbar snackbar= Snackbar.Make("已添加到好友列表");
            //snackbar.Show();
            Toast.Make("添加成功").Show();
            //button.DisplaySnackbar("添加成功");
        }
		
    }
}