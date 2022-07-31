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

        /**
         创建StackPanel  并将socket和 stackpanel 绑定 
         
         */
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
         void  ReceiveAsyncByOneClient(Socket client,StackPanel panel)
        {
            byte[] buf = new byte[1024];
            while (true)
            {
         
                try
                {

                    Task<int> recvTask = client.ReceiveAsync(buf, SocketFlags.None);
                    int len = recvTask.Result;
                    string s = Encoding.Default.GetString(buf, 0, len);
                    changeText(s, panel);
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
          
         
            //ReceiveAsyncByOneClient(client,panel);
        }
        private  StackPanel AddElement()
        {
            NewDialogModel newDialogModel = new NewDialogModel(); ;
            return newDialogModel.AddElement(stackPanel, sendText); ;
        }
   
        private void changeText(string s,StackPanel panel) 
        {
            Dispatcher.UIThread.Post(
                () =>
                    {
                        panel.FindByName<TextBlock>("ShowRecvText").Text=s;
                        //block.Text = s;
                    }
                );

        }
        private void sendText(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            StackPanel stackpanel = (StackPanel)btn.Parent;
            TextBox text= stackpanel.FindByName<TextBox>("Content");
            byte[] data = Encoding.Default.GetBytes(text.Text);
            clientDict[stackpanel].SendAsync(data,SocketFlags.None);

        }

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
