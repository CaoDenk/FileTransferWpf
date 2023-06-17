using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using p2pchat.HttpRequest;
using p2pchat.Global;
using Plugin.LocalNotification;
using static p2pchat.Utils.Utils;
using p2pchat.UdpCommunication;

namespace p2pchat.ExeTask
{
    internal class ListenTask
    {
     
        public static Socket socket {  get; private set; }

        public static int port { get; private set; }

        //public static byte[] localIpv6 { get; private set; }
        public static IPAddress localIpv6Address { get; private set; }

        static void Bind()
        {

            IPEndPoint ep = new IPEndPoint(IPAddress.IPv6Any, 0);
            socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(ep);
            var ipep = (IPEndPoint)socket.LocalEndPoint;
            port = ipep.Port;

        }
        static async void Listen()
        {

            while (true)
            {
                try
                {
                    
                    EndPoint remoteEp = new IPEndPoint(IPAddress.IPv6Any, 0);
                    byte[] buffer = new byte[Config.BUF_SIZE];
                    int len= socket.ReceiveFrom(buffer,ref remoteEp);
                    RecvMsgHandle msgHandle = new RecvMsgHandle(buffer, len, (IPEndPoint)remoteEp);
                    Task.Run(msgHandle.Handle);
                }
                catch (Exception ex)
                {
                    //待处理
                    //记录错误日志
              

                }
            }
        }
        /// <summary>
        /// 我觉得应该不需要获取所有公网ipv6，一个设备会有多个公网ipv6，临时ipv6也能够完成通信
        /// 所以只获取一个ipv6 应该也是可行的
        /// </summary>
        /// <returns></returns>
        static IPAddress GetLocalIpv6()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface ni in interfaces)
            {

                IPInterfaceProperties ipProps = ni.GetIPProperties();

                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        if (addr.Address.IsIPv6LinkLocal || addr.Address.ToString().StartsWith("::"))
                        {
                            continue;
                        }

                        return addr.Address;
                    }
                }

            }
            return null;
        }


        static async void ObersevIpv6()
        {

            while (true)
            {
                IPAddress newLocalIpv6 = GetLocalIpv6();
                if (newLocalIpv6 == null)
                {
                    localIpv6Address = null;


                }
                else if (localIpv6Address == null || !CmpBytes(localIpv6Address.GetAddressBytes(), newLocalIpv6.GetAddressBytes()))
                {
                    localIpv6Address = newLocalIpv6;
                    UpdateIpv6.HttpPutIpv6();
                }

                await Task.Delay(1500);
            }
        }






        public static async void Init()
        {

            await Task.Run(Bind);
            Task.Run(Listen);
            Task.Run(ObersevIpv6);

        }





    }
}
