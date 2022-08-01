using FileTransfer.Models;
using FileTransfer.Tools;
using System;
using System.Net;
using System.Net.Sockets;
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

        public bool isConnected => socket.Connected;
        private Socket socket;
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
                SendHandle sendHandle = new SendHandle();
                foreach (var file in filePath)
                {
                  
                    Task task = new Task(
                        () =>
                        {

                            //发送文件
                            //FileInfo info = new FileInfo(file);
                            //FileStream fileStream=   File.OpenRead(file);
                            //byte[] bytes = new byte[1024 * 1024 * 4];
                            //BitConverter.TryWriteBytes(buf, 1);
                            ////Span<byte> span = new Span<byte>(bytes);

                            //int len=fileStream.Read(bytes, 4, bytes.Length);
                            //fileStream.Read(bytes,3,)
                            //bytes[3] = 1;
                            //while((len=>)
                            //socket.SendFileAsync(filePath[0]);
                            sendHandle.FileHandle(file, socket);
                        }
                        );
                   task.Start();
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
                byte[] data = SendHandle.AddTextInfoHeader(s);
                socket.SendAsync(data, SocketFlags.None);
            }
            else
            {
                MyMessage.show("error", "disconnected ");
            }
        }



        void receiveAsync()
        {

            while (true)
            {

                try
                {

                    Task<int> recvTask = socket.ReceiveAsync(buf, SocketFlags.None);
                    int len = recvTask.Result;
                    RecvHandle recvHandle=new RecvHandle(buf,len);
                    
                    switch(recvHandle.GetDataType())
                    {
                        case InfoHeader.TEXT:
                            changeText(recvHandle.GetProcessedText());
                            break;
                        case InfoHeader.FILE:
                            break;
                        default:
                            break;
                    }

                    //string s = Encoding.Default.GetString(buf, 4, len-4);
                    //changeText(s);
                }
                catch (Exception e)
                {
                    socket.Close();
                    changeColor(false);
                    return;
                }
            }

        }
        /// <summary>
        /// 连接服务端socket
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            
            if(socket!=null&&socket.Connected)
            {
                MyMessage.show("warning", "socked has been connected!,don't repeat the opetation!");
                return true;
            }

            try
            {
               
                EndPoint endPoint = new System.Net.IPEndPoint(IPAddress.Parse(ip), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                new Task(receiveAsync).Start();

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
