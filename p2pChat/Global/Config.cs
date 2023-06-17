using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.Global
{
    internal class Config
    {
        public const int BUF_SIZE = 4096;

        //readonly static string ANDROID_STORAGE=Environment.GetEnvironmentVariable("ANDROID_STORAGE");
        //public readonly static string DOWNLOAD_PATH=$"{ANDROID_STORAGE}/emulated/0/Download";

#if ANDROID
        public readonly static string DOWNLOAD_PATH = $"{Environment.GetEnvironmentVariable("ANDROID_STORAGE")}/emulated/0/Download";
        public readonly static string CHAT_HISTORY_DB_PATH = $"{Environment.GetEnvironmentVariable("HOME")}/chat_history.db";
        public readonly static string FRIENDS_DB_PATH = $"{Environment.GetEnvironmentVariable("HOME")}/friends.db";

#else
        public readonly static string DOWNLOAD_PATH = "D://download";
        public readonly static string FRIENDS_DB_PATH = "friends.db";
        public readonly static string CHAT_HISTORY_DB_PATH = "D:";
#endif

        public const int HEAD_LEN = 36;
    }
}
