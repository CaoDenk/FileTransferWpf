using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using p2pchat.Global;
using p2pchat.HttpRequest;
using p2pchat.Init;
using p2pchat.pojo;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using User = p2pchat.pojo.User;

namespace p2pchat.Crud
{
    internal class FriendsDao
    {
        public static async Task<List<User>> GetFriendsFromSqlite()
        {

          return await  Task.Run(() =>
            {

                List<User> friends = new List<User>();
                using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");
                connection.Open();
                SqliteCommand sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = "select id,uid,username,alias from friends";
                SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    User user = new User();
                    user.id = sqliteDataReader.GetInt32(0);
                    user.uid = sqliteDataReader.GetString(1);
                    user.username = sqliteDataReader.GetString(2);
                    if (!sqliteDataReader.IsDBNull(3))
                    {
                        user.alias = sqliteDataReader.GetString(3);
                    };
                    friends.Add(user);
                }
                return friends;
            });
          
        }
        /// <summary>
        /// 先判断用户是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
       public static bool Exists(string uid)
        {
            using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");
            connection.Open();
            SqliteCommand sqliteCommand = connection.CreateCommand();
            sqliteCommand.CommandText = $"select count(1) from friends where uid='{uid}' limit 1";
            SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
            sqliteDataReader.Read();
            return sqliteDataReader.GetInt32(0)>0;

        }

        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="result"></param>
       public static void InsertUser(QueryUserResult result)
        {
            Task.Run(() =>
            {
                using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");
                connection.Open();
                SqliteCommand sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = $"insert  into friends (uid,username ) values('{result.uid}','{result.username}')";
                sqliteCommand.ExecuteNonQuery();
            });
      

        }


       public static void DeleteFriendByUid(string uid)
        {
            Task.Run(() =>
            {

                using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");
                connection.Open();
                SqliteCommand sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = $"delete from friends where uid='{uid}'";
                sqliteCommand.ExecuteNonQuery();

            });
              

        }

        public static  async Task<User> QueryUsrByUid(string uid)
        {
          return await Task.Run(() =>
            {

                using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");
                connection.Open();
                SqliteCommand sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = $"select id,username,alias from friends where uid='{uid}' limit 1";

                using SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
              
                User user = new User();
                user.uid = uid;

                if(sqliteDataReader.Read())
                {
                    user.id = sqliteDataReader.GetInt32(0);
                    //user.uid = sqliteDataReader.GetString(1);
                    user.username = sqliteDataReader.GetString(1);
                    if (!sqliteDataReader.IsDBNull(2))
                    {
                        user.alias = sqliteDataReader.GetString(3);
                    };
                }
              
                return user;
               
            });


        } 


    }
}
