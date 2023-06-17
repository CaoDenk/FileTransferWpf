using p2pchat.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.UdpCommunication
{
    /// <summary>
    /// 消息报文头
    /// 0..4  
    /// 4..36
    /// 
    /// </summary>
    
    //const 
    class AddMsgWithInfo
    {
       //static byte[] TextBytes=>BitConverter.GetBytes((int)DataType.TEXT).ToArray().Reverse().ToArray();

        public static byte[] AddInfo(string msg)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
            byte[] buf=new byte[msgBytes.Length+Config.HEAD_LEN];

            BitConverter.TryWriteBytes(buf, (int)DataType.TEXT);

            Array.Copy(GlobalVar.uuidBytes, 0, buf, 4, 32);

            Array.Copy(msgBytes,0,buf, Config.HEAD_LEN,msgBytes.Length);

            return buf;
        }


       



    }
}
