using Microsoft.Data.Sqlite;
using p2pchat.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.Init
{
    internal class DbOp
    {



        /// <summary>
        /// 在第一次创建的时候，检测数据库是否存在
        /// </summary>
        public static void  Init()
        {

            if(!File.Exists(Config.FRIENDS_DB_PATH))
            {
                //File.Delete(Config.FRIENDS_DB_PATH);
                using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");

                connection.Open();

                var sqlcomm = connection.CreateCommand();

                sqlcomm.CommandText = "create table if not exists  friends (id integer primary key autoincrement,uid char(32),username varchar(50),alias varchar(50) )";

                //sqlcomm.CommandText = "insert into friends(id,name) values(2,'denk')";
                sqlcomm.ExecuteNonQuery();
            }

            //if (!File.Exists(Config.FRIENDS_DB_PATH))
            //{
            //    using var connection = new SqliteConnection($"Data Source = {Config.FRIENDS_DB_PATH}");

            //    connection.Open();

            //    var sqlcomm = connection.CreateCommand();

            //    sqlcomm.CommandText = "create table if not exists  friends (id integer primary key autoincrement,uid char(32),username varchar(50),alias varchar(50))";

            //    sqlcomm.ExecuteNonQuery();
            //}



        }

    }
}
