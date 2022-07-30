using Avalonia.Threading;
using FileTransfer.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.ViewModels
{
    internal class ClientWindowViewModel
    {

        string ip="127.0.0.1";
        public string Ip{ get { return ip; } set { ip = value; } }
        int port = 8081;
        public int Port{ get { return port; } set { port = value; } }

        public delegate void ChangeText(string s);
        public delegate void ChangeColor(bool b);
        bool isSuddenlyDisconnected = false;
        public bool isConnected => socket.Connected;
        private Socket socket;
        private Socket endPointSocket;
        byte[] buf = new byte[1024];
        public ChangeText changeText;
        ChangeColor changeColor;
        public ClientWindowViewModel(ChangeColor changeColor)
        {
            this.changeColor = changeColor;
        }


        public void sendFile(string[] filePath)
        {
            if (filePath == null)
            {
                MyMessage.show("error", "file is empty!");
                return;
            }
            if (socket.Connected)
            {
                foreach(var file in filePath)
                {
                    Task task = new Task(
                        () =>
                        {
                            FileInfo info = new FileInfo(file);
                            FileStream fileStream=   File.OpenRead(file);
                            byte[] bytes = new byte[1024 * 1024 * 4];
                            BitConverter.TryWriteBytes(buf, 1);
                            //Span<byte> span = new Span<byte>(bytes);
                            
                            int len=fileStream.Read(bytes, 4, bytes.Length);
                            //fileStream.Read(bytes,3,)
                            //bytes[3] = 1;
                            //while((len=>)


                        }
                        );
                   task.Start();


                //socket.SendFileAsync(filePath[0]);
                }
         
            }
            else
            {
                MyMessage.show("error", "disconnected!");
            }
        }
        public void sendText(string s)
        {
            if (s == null)
            {
                MyMessage.show("error", "input is empty!");
                return;
            }

            if (socket.Connected)
            {
                byte[] data = Encoding.Default.GetBytes(s);
                socket.SendAsync(data, SocketFlags.None);
            }
            else
            {
                MyMessage.show("error", "disconnected ");
            }
        }
        public void accept()
        {
            socket.BeginAccept(asyncResult =>
            {               
                endPointSocket = socket.EndAccept(asyncResult);

                changeColor(true);
                beforeReceiveAsync(asyncResult);
            }, null);

        }

        /*
         封装太多层不利于将来维护，代码尽量简单
         
         */
        //void endReceiveAsync(IAsyncResult asyncResult)  
        //{
        //    try
        //    {
        //        int len = endPointSocket.EndReceive(asyncResult);

        //        changeText(Encoding.Default.GetString(buf, 0, len));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}

        void beforeReceiveAsync(IAsyncResult asyncResult)
        {
            endPointSocket.BeginReceive(buf, 0, 1024, SocketFlags.None, asyncResult =>
            {
                try
                {
                    int len = endPointSocket.EndReceive(asyncResult);
                    changeText(Encoding.Default.GetString(buf, 0, len));
                    beforeReceiveAsync(asyncResult);
                }
                catch (Exception)
                {
                    Dispatcher.UIThread.Post(() => MyMessage.show("disconnected", "connection closed"));//在这捕获下，
                    endPointSocket.Close();
                    changeColor(false);
                    accept();
                }
            }, null);

        }


        void receiveAsync()
        {
            socket.BeginReceive(
                 buf, 0, buf.Length, SocketFlags.None, asyncResult =>
                 {
                     try
                     {
                         int len = socket.EndReceive(asyncResult);

                         Dispatcher.UIThread.Post(() => {
                             changeText(Encoding.Default.GetString(buf, 0, len));
                         });
                         receiveAsync();
                     }
                     catch (Exception)
                     {
                         isSuddenlyDisconnected = true;
                         changeColor(false);
                     }
                 }, null
                 );

        }
        public bool Connect()
        {
            
            if(socket!=null&&socket.Connected)
            {
                MyMessage.show("warning", "socked has been connected!,don't repeat the opetation!");
                return true;
            }

            try
            {
                if (isSuddenlyDisconnected)
                {
                    socket.Close();
                }

                EndPoint endPoint = new System.Net.IPEndPoint(IPAddress.Parse(ip), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                receiveAsync();

            }
            catch (Exception e)
            {
                //MyMessage.show("error", e.Message);
               
                return false;
            }
            bool res=socket.Connected;
            return res;
        }
        public void CloseSocket()
        {
  
            socket.Close();
            
        }



    }
}
