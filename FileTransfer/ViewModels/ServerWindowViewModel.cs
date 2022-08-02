using Avalonia.Controls;
using Avalonia.Threading;
using FileTransfer.Models;
using FileTransfer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using System.IO;
using FileTransfer.Dialogs;

namespace FileTransfer.ViewModels
{
 
    internal class ServerWindowViewModel
    {
        int port = 8081;
        public int Port { get { return port; } set { port = value; } }
        public bool IsBound => serverSocket.IsBound;
        private Socket serverSocket;//服务端socket 
        //ChangeText changeText;
        StackPanel stackPanel;
        Task task;
        //List<Socket> clients = new List<Socket>();//储存客户端连接所产生的socket
        Dictionary <StackPanel, Socket> clientDict=new Dictionary<StackPanel,Socket>();
        Window window;
        public ServerWindowViewModel( StackPanel stackPanel, Window window)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //this.changeText = changeText;
            this.stackPanel = stackPanel;
            this.window = window;
        }

        /// <summary>
        /// 同时接受多个请求
        /// </summary>
        void acceptMany()
        {
         
            while (true)
            {

                Task<Socket> mytask = serverSocket.AcceptAsync();
                Socket client;
                try
                {                
                    client = mytask.Result;
                }catch(Exception e)
                {
                    return;
                }
                
                //clients.Add(client);
                StackPanel panel = AddElement();
                clientDict.Add(panel, client);
                try
                {
                    new Task(() =>
                    {
                        ReceiveAsyncByOneClient(client, panel);
                    }).Start();
                   
                }catch(Exception e)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        stackPanel.Children.Remove(panel);

                    });

                    clientDict.Remove(panel);
                    client.Close();
                }
                   
            }
        }


        public void Bind()
        {

            EndPoint endPoint = new System.Net.IPEndPoint(IPAddress.Any, Port);
            try
            {
                serverSocket.Bind(endPoint);
            }catch(Exception e)
            {
                MyMessage.show("error", "can't bind the port,pls input anther port!");
                return;
            }
        
            serverSocket.Listen();
            task = new Task(acceptMany);
            task.Start();
        }
        /// <summary>
        /// 异步接受文本数据，偏移四个字节
        /// </summary>
        /// <param name="client"></param>
        /// <param name="panel"></param>
         async void  ReceiveAsyncByOneClient(Socket client,StackPanel panel)
        {
            byte[] buf = new byte[Config.FileBufSize];
            FileStream fileStream=null;
            string saveFilePath;
           // int totalSize = 0;
            List<int> ints = new List<int>();
            while (true)
            {
         
                try
                {
                    Task<int> recvTask =  client.ReceiveAsync(buf, SocketFlags.None);
                    int len = recvTask.Result;
                    RecvHandle recvHandle = new RecvHandle(buf, len);

                    int type;
                    switch (type=recvHandle.GetDataType())
                    {
                        case InfoHeader.TEXT:
                            changeText(recvHandle.GetProcessedText(),panel);
                            break;
                        case InfoHeader.FILE:
                            MyFiles myFiles = new MyFiles();
                            saveFilePath=myFiles.save(window, recvHandle.GetFileName());
                            //recvHandle.FileHandle(s,client,len);
                            //changeText(s, panel);
                            //recvHandle.SaveFile(s, client, window);
                            fileStream= File.Create(saveFilePath);
                            break;
                        case InfoHeader.CONTINUE_RECV:
                            ints.Add(BitConverter.ToInt32(buf[4..]));
                            //totalSize += len ;
                         
                            fileStream.Write(buf, 16, len-16);
                            //fileStream.Flush();
                            break;
                        case InfoHeader.SEND_FINISHED:
                            ints.Add(BitConverter.ToInt32(buf[4..]));
                            int size = BitConverter.ToInt32(buf[8..]);
                          //  totalSize += size;
                            
                            fileStream.Write(buf, 16, size);
                            fileStream.Flush();
                            fileStream.Close();
                            ints.Clear();
                            break;
                        default:
                            throw new Exception("info error");
                            //break;
                    }
                    //string s = Encoding.Default.GetString(buf, 4, len-4);//偏移4个字节
                    //changeText(s, panel);
                }
                catch (Exception e)
                {
                    Dispatcher.UIThread.Post(
                   () =>
                   {
                       stackPanel.Children.Remove(panel);
                   });
                    CloseAndRemove(client, panel);
                    return;
                }


            }
        }
        private  StackPanel AddElement()
        {
            NewDialogModel newDialogModel = new NewDialogModel(); ;
            return newDialogModel.AddElement(stackPanel, sendText); ;
        }

      
        /// <summary>
        /// 显示内容到界面指定的元素容器
        /// </summary>
        /// <param name="s"></param>
        /// <param name="panel"></param>
        private void changeText(string s,StackPanel panel) 
        {
            Dispatcher.UIThread.Post(
                () =>
                    {
                        panel.FindByName<TextBlock>("ShowRecvText").Text = s;
                    }
                );

        }
     
        /// <summary>
        /// 发送文本事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendText(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            StackPanel stackpanel = (StackPanel)btn.Parent;
            TextBox text= stackpanel.FindByName<TextBox>("Content");
            byte[] data = SendHandle.AddTextInfoHeader(text.Text);
            clientDict[stackpanel].SendAsync(data,SocketFlags.None);
        }
        /// <summary>
        /// 先关闭客户端，使其断开，再关闭服务端
        /// </summary>
        public void CloseSocket()
        {
            foreach (StackPanel s in clientDict.Keys)
            {
                //clientDict[s].Disconnect(true);
                clientDict[s].Close();
            }

            serverSocket.Close();
        }

        void CloseAndRemove(Socket client,StackPanel panel)
        {
            clientDict.Remove(panel);
            client.Close();
        }

    }
}
