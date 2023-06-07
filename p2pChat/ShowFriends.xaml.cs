using Microsoft.Data.Sqlite;
using System.Text;

namespace p2pchat;

public partial class ShowFriends : ContentPage
{
	public ShowFriends()
	{
		InitializeComponent();
	}
    int count = 0;

    List<string> friends = new List<string>()
    {
        "xiaoming",
        "小红",
        "ggg",
        "afg",
        "xiaole"
    };
    private void AddFriend(object sender, EventArgs e)
    {

        string data_path = Environment.GetEnvironmentVariable("HOME");
        using (var connection = new SqliteConnection($"Data Source = {data_path}/friends.db"))
        {
            connection.Open();

            var sqlcomm = connection.CreateCommand();

            //sqlcomm.CommandText = "create table friends (id int,name varchar(50) )";
            int no=count%friends.Count;
            sqlcomm.CommandText = $"insert into friends(id,name) values({count},'{friends[no]}')";
            sqlcomm.ExecuteNonQuery();
            count++;

        }




    }

    private void CreateDB(object sender, EventArgs e)
    {
        string data_path = Environment.GetEnvironmentVariable("HOME");
        using (var connection = new SqliteConnection($"Data Source = {data_path}/friends.db"))
        {
            
            connection.Open();

            var sqlcomm = connection.CreateCommand();

            sqlcomm.CommandText = "create table friends (id int,name varchar(50) )";

            //sqlcomm.CommandText = "insert into friends(id,name) values(2,'denk')";
            sqlcomm.ExecuteNonQuery();


            //sqlcomm.CommandText = "select * from friends";
            ////sqlcomm.ExecuteNonQuery(
            //using (var reader = sqlcomm.ExecuteReader())
            //{

            //    while (reader.Read())
            //    {

            //        var name = reader.GetString(1);
            //        Console.WriteLine(name);
            //    }

            //}
        }


    }

    private void Query(object sender, EventArgs e)
    {
        string data_path = Environment.GetEnvironmentVariable("HOME");
        using (var connection = new SqliteConnection($"Data Source = {data_path}/friends.db"))
        {
            connection.Open();

            var sqlcomm = connection.CreateCommand();

            sqlcomm.CommandText = "select * from friends";
            //sqlcomm.ExecuteNonQuery(
            using (var reader = sqlcomm.ExecuteReader())
            {

                while (reader.Read())
                {

                    var name = reader.GetString(1);
                    Label label = new Label();
                    label.Text = name;
                    labelsStack.Add(label);
                }

            }
        }

    }

    private void GetDataPath(object sender, EventArgs e)
    {

        //var dic = Environment.GetEnvironmentVariables();
        //List<string> path = new List<string>();
        //foreach (var key in dic)
        //{

        //    var str = $"{key}:{dic[key]}";
        //    Console.WriteLine(str);
        //    path.Add(str);

        //}


        string  data_path= Environment.GetEnvironmentVariable("ANDROID_DATA");
        Label label = new Label();
        label.Text = data_path;
        labelsStack.Add(label);
    }

    private async void ShowPermissions(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

        if (status == PermissionStatus.Granted)
        {
           await DisplayAlert("权限", "运行", "ok");
        }else
        {

            await DisplayAlert("权限", "不允许", "ok");
            await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

    }


    private  void TestPermissions(object sender, EventArgs e)
    {
        string data_path = Environment.GetEnvironmentVariable("HOME");

        string filepath = $"{data_path}/test.txt";
        using (FileStream fileStream = File.Create(filepath))
        {
            fileStream.Write(Encoding.UTF8.GetBytes("test"));
        }
        
        DirectoryInfo directoryInfo = new DirectoryInfo(data_path);
        var files = directoryInfo.GetFiles();
        foreach(var file in files)
        {
            Label label = new Label();
            label.Text = file.Name;

            labelsStack.Add(label);
        }

       
        
        

    }
}