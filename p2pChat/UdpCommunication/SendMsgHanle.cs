using p2pchat.ExeTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.UdpCommunication
{
    internal class SendMsgHanle
    {
        string msg;
        IPEndPoint address;
        public SendMsgHanle(string msg, IPEndPoint address)
        {
            this.msg = msg;
            this.address = address;
        }

        public void Send()
        {
            byte[] buffer = AddMsgWithInfo.AddInfo(msg);
            ListenTask.socket.SendToAsync(buffer, address);

        }



    }
}
