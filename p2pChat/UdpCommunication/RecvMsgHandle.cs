
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

        public  async void Handle()
        {

            DataType dataType = (DataType)BitConverter.ToInt32(buf);
            switch (dataType)
            {
                case DataType.TEXT:
                    UserMsg userMsg = ParseMsgBuf.Parse(buf, len, address);
              
                    User user =await FriendsDao.QueryUsrByUid(userMsg.uid);

                    ChatHistoryDao chatHistoryDao = new ChatHistoryDao(userMsg.uid, userMsg.uid);
               
                    if (user.username == null)
                    {
                        Task.Run(chatHistoryDao.CreateChatHistoryTable);

                        MsgPush.Push("陌生人的消息", userMsg.msg);
                    }
                    else
                    {
                        MsgPush.Push($"{user.username}", userMsg.msg);
                    }
                    Task.Run(chatHistoryDao.InsertMsg);
                    DisplayRecvMsg.Show(userMsg.msg);
                    
                    break;
                case DataType.FILE:
                    break;
            }
         
        

        }


        public void ShowMsg()
        {
            //if(userMsg.address != null)
            //{


            //}


        }

  

    }
}
