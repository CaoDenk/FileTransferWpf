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
        public ServerWindowViewModel( StackPanel stackPanel)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //this.changeText = changeText;
            this.stackPanel = stackPanel;
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
        
            serverSocket.Listen(10);
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
            byte[] buf = new byte[1024];
            while (true)
            {
         
                try
                {
                    Task.Delay(1000);
                    Task<int> recvTask =  client.ReceiveAsync(buf, SocketFlags.None);
                    int len = recvTask.Result;
                    RecvHandle recvHandle = new RecvHandle(buf, len);

                    switch (recvHandle.GetDataType())
                    {
                        case InfoHeader.TEXT:
                            changeText(recvHandle.GetProcessedText(),panel);
                            break;
                        case InfoHeader.FILE:
                            changeText(recvHandle.GetFileName(), panel);
                            break;
                        default:
                            break;
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
            byte[] data = SendHandle.AddInfoHeader(text.Text);
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
