using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.init
{
    internal class DbOp
    {



         static   DbOp()
        {
            string data_path = Environment.GetEnvironmentVariable("HOME");
            using (var connection = new SqliteConnection($"Data Source = {data_path}/friends.db"))
            {

                connection.Open();

                var sqlcomm = connection.CreateCommand();

                sqlcomm.CommandText = "create table friends (id int primary key auto_increment,uuid varchar(32),name varchar(50),scope varchar(50) )";

                //sqlcomm.CommandText = "insert into friends(id,name) values(2,'denk')";
                sqlcomm.ExecuteNonQuery();

            }



        }

    }
}
