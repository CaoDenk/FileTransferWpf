using System.Net.Sockets;

namespace FileTransfer.Models
{
    internal class Client
    {

        public string Ip { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 8080;
        public Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    }
}
