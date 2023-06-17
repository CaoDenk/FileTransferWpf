using p2pchat.pojo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.Global
{
    /// <summary>
    /// 储存全局变量
    /// </summary>
    internal class GlobalVar
    {
        public static string uuid;
        public static string username;
        public static byte[] uuidBytes=>Encoding.UTF8.GetBytes(uuid);

        //public static User chatWith;
        public static string uidChatWith;
    }
}
