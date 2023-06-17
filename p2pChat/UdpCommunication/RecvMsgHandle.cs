
using p2pchat.Crud;
using p2pchat.Dao;
using p2pchat.Global;
using p2pchat.pojo;
using p2pchat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.UdpCommunication
{
    internal class RecvMsgHandle
    {

        //UserMsg userMsg;

        byte[] buf;
        int len;
        IPEndPoint address;
        public RecvMsgHandle(byte[] buf, int len, IPEndPoint address)
        {
            this.buf = buf;
            this.len = len;
            this.address = address;
        }

        public  void Handle()
        {

            UserMsg userMsg = ParseMsgBuf.Parse(buf, len, address);

            User user = FriendsDao.QueryUsrByUid(userMsg.uid).Result;

            ChatHistoryDao chatHistoryDao = new ChatHistoryDao(userMsg.uid,userMsg.uid);

            if(user.username==null)
            {
                Task.Run(chatHistoryDao.CreateChatHistoryTable);
#if ANDROID

                MsgPush.Push("陌生人的消息", userMsg.msg);
#endif

            }
            else
            {
#if ANDROID
                MsgPush.Push($"{user.username}", userMsg.msg);
#endif
            }

            Task.Run(chatHistoryDao.InsertMsg);
            //DataType  dataType=(DataType)BitConverter.ToInt32(buf);
            //switch (dataType)
            //{
            //    case DataType.TEXT:
            //        userMsg=ParseMsgBuf.Parse(buf,len, address);
            //        //MsgPush.Push("消息", userMsg.msg);
            //        Console.WriteLine($"收到消息{userMsg.msg}");
            //        break;
            //    case DataType.FILE:
            //        break;
            //}
            DisplayRecvMsg.Show(userMsg.msg);

        }


        public void ShowMsg()
        {
            //if(userMsg.address != null)
            //{


            //}


        }

  

    }
}
