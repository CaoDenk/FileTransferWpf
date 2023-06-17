using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using p2pchat.Global;
namespace p2pchat.Dao
{
    internal class ChatHistoryDao
    {

        string uid;
        string msg;
        public ChatHistoryDao(string uid,string msg) { 
            this.uid = uid;
            this.msg = msg;

        }

        public   void CreateChatHistoryTable()
        {
            using var connection = new SqliteConnection($"Data Source = {Config.CHAT_HISTORY_DB_PATH}");
            connection.Open();
            SqliteCommand sqliteCommand = connection.CreateCommand();
            sqliteCommand.CommandText = $"create table  if not exists {uid} (id INTEGER primary key autoincrement,int type default 0,content text )";
            sqliteCommand.ExecuteNonQuery();
           

        }
        //static bool  TableIsExists(SqliteConnection connection, string uid)
        //{
        //    SqliteCommand sqliteCommand = connection.CreateCommand();
        //    sqliteCommand.CommandText = $"SELECT COUNT(1) FROM sqlite_master WHERE TYPE='table' AND name='{uid}' LIMIT 1;";
        //    using SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
        //    sqliteDataReader.Read();
        //    return sqliteDataReader.GetInt32(0)>0;
        //}
        public  void InsertMsg()
        {
            using var connection = new SqliteConnection($"Data Source = {Config.CHAT_HISTORY_DB_PATH}");
            connection.Open();
            SqliteCommand sqliteCommand = connection.CreateCommand();
            sqliteCommand.CommandText = $"insert into {uid} (type,content) values(0,'{msg}')";
            sqliteCommand.ExecuteNonQuery();


        }




    }
}
