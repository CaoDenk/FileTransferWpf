using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace p2pchat.crud
{
    internal class GetFriends
    {
        List<string> friends;


        void GetFriendsFromSqlite()
        {

            //sqlite3 sqlite3 = new sqlite3();

            using (var connection = new SqliteConnection("Data Source = hello.db"))
            {
                connection.Open();


                 
            }
        }



    }
}
