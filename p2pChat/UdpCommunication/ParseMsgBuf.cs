using p2pchat.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.UdpCommunication
{

    public class UserMsg
    {
        public string uid;
        public string msg;
        public IPEndPoint address;

    }
    internal class ParseMsgBuf
    {

        //string uid;
        public static UserMsg Parse(byte[] data,int len,IPEndPoint address)
        {
            string fromUid = Encoding.UTF8.GetString(data, 4, 32);

            string msg=Encoding.UTF8.GetString(data,Config.HEAD_LEN, len-Config.HEAD_LEN);
            return new UserMsg
            {
                uid = fromUid,
                msg = msg,
                address = address
            };

        }


    }
}
