using Avalonia.Controls;
using FileTransfer.Elements;
using FileTransfer.GlobalConfig;
using FileTransfer.Header;
using FileTransfer.Models;
using FileTransfer.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia.Threading;
using Avalonia.Interactivity;
using MessageBox.Avalonia.Enums;
using static FileTransfer.MessageBox;
//using FileTransfer.Tools;
namespace FileTransfer.ViewModels
{
    internal class ServerWindowViewModel
    {
        Server server = new Server();

        public int Port { get { return server.Port; } set { server.Port = value; } }
        public Socket ServerSocket { get { return server.ServerSocket; } set { server.ServerSocket = value; } }
        public bool IsBound => ServerSocket.IsBound;

        public Dictionary<byte[], UUIDRecvFileModel> uuidRecvDict = new Dictionary<byte[], UUIDRecvFileModel>(new ByteCmp());


        //之所以创建全局，而不是局部变量，以防强制关闭的时候，得关闭流
        Dictionary<StackPanel, Socket> stackPanelSocketDict = new Dictionary<StackPanel, Socket>();
        HashSet<Socket> clientsSocket = new HashSet<Socket>();

        /// <summary>
        /// 绑定端口
        /// </summary>
        public bool Bind()
        {

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, Port);
            try
            {
                ServerSocket.Bind(endPoint);
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭流，关闭socket
        /// </summary>
        public void Close()
        {
            foreach (UUIDRecvFileModel u in uuidRecvDict.Values)
            {
                if (u.stream != null)
                {
                    u.stream.Close();
                }


            }
            foreach (Socket socket in stackPanelSocketDict.Values)
            {
                socket.Close();
            }

            ServerSocket.Close();

        }

        public void Listen(StackPanel panel, Window parent)
        {
            ServerSocket.Listen();
            Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        try {
                            Socket client = ServerSocket.Accept();
                            //clients.
                            ShowRecvProgress showRecvProgress = AddElements.AddElement(panel, SendText);

                            stackPanelSocketDict.Add(showRecvProgress.stackPanelParent, client);

                            Task.Factory.StartNew(() =>
                            {
                                ReceiveByOneClient(panel, client, showRecvProgress, parent);
                            }
                           );

                        } catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                            //return;
                            break;
                        }

                    }
                }
                );



        }
        async void ReceiveByOneClient(StackPanel parentPanel, Socket client, ShowRecvProgress showRecvProgress, Window parent)
        {
            byte[] buf = new byte[Config.FILE_BUFFER_SIZE];


            byte[] uuidbytes = null;
            HashSet<byte[]> set = new HashSet<byte[]>(new ByteCmp());


            while (true)
            {

                try
                {
                    int len = client.Receive(buf);
                    int type = RecvHandle.GetDataType(buf);

                    switch (type)
                    {
                        case InfoHeader.TEXT:
                            ShowContent(buf, showRecvProgress.showRecvText, len);
                            break;
                        case InfoHeader.FILE:
                            RecvFile recv = RecvHandle.GetRecvFileInfo(buf);
                            string msg = string.Format("是否接收文件{0},文件大小{1}字节", recv.filename, recv.filesize);
                            MessageBoxResult messageBoxResult = await MessageBox.Show(msg, "消息", MessageBoxButtons.YesNo);
                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                SaveFileDialog saveFileDialog = new SaveFileDialog();
                                saveFileDialog.InitialFileName = recv.filename;
                                var res = await saveFileDialog.ShowAsync(parent);
                                if (res != null)
                                {

                                    //创建文件
                                    FileStream fileStream = File.Create(res);

                                    UUIDRecvFileModel uUIDFileModel = new UUIDRecvFileModel();
                                    uUIDFileModel.filesize = recv.filesize;
                                    uUIDFileModel.packnum = recv.packagenum;
                                    uUIDFileModel.stream = fileStream;
                                    uUIDFileModel.start = DateTime.Now;
                                    uUIDFileModel.recvSocket = client;

                                    uUIDFileModel.showPercent = AddElements.AddProgressFromStackPanel(showRecvProgress.stackPanelParent);
                                    //存在dict中
                                    uuidRecvDict.Add(recv.uuidbytes, uUIDFileModel);

                                    //如果接收多个文件
                                    set.Add(recv.uuidbytes);

                                    byte[] data = SendHandle.AllowRecv(recv.uuidbytes);
                                    client.Send(data, SocketFlags.None);
                                    break;
                                }
                            }
                            byte[] recvbytes = SendHandle.RefuseRecv(recv.uuidbytes);
                            client.Send(recvbytes, SocketFlags.None);
                            break;
                        case InfoHeader.CONTINUE_RECV:
                            uuidbytes = buf[8..16];
                            int packnum = BitConverter.ToInt32(buf, 4);
                            if (packnum != uuidRecvDict[uuidbytes].packOrder)
                            {
                                client.Send(SendHandle.SendResendPack(uuidbytes, uuidRecvDict[uuidbytes].packOrder, uuidRecvDict[uuidbytes].hasRecvSize));
                                break;
                            }
                            uuidRecvDict[uuidbytes].packOrder++;
                            uuidRecvDict[uuidbytes].hasRecvSize += len - 16;
                            double percent = uuidRecvDict[uuidbytes].hasRecvSize * 1.0 / uuidRecvDict[uuidbytes].filesize * 100;

                            AddElements.SetBarValue(uuidRecvDict[uuidbytes].showPercent, percent);

                            uuidRecvDict[uuidbytes].stream.Write(buf, 16, len - 16);
                            uuidRecvDict[uuidbytes].stream.Flush();

                            //发送接受成功
                            byte[] recvOk = SendHandle.SendRecvOk(uuidbytes);
                            client.Send(recvOk, SocketFlags.None);

                            break;
                        case InfoHeader.FINISHED:

                            uuidbytes = buf[8..16];
                            if (!uuidRecvDict.ContainsKey(uuidbytes))
                            {
                                break;
                            }
                            if (uuidRecvDict[uuidbytes].hasRecvSize == uuidRecvDict[uuidbytes].filesize)
                            {
                                TimeSpan duration = DateTime.Now - uuidRecvDict[uuidbytes].start;
                                Task.Factory.StartNew(() =>
                                {
                                    MessageBox.Show("接收完成,花费" + duration.TotalSeconds + "s");
                                });
                                client.Send(SendHandle.SendCloseSend(uuidbytes));
                                uuidRecvDict[uuidbytes].stream.Close();


                                ShowPercent showPercent = uuidRecvDict[uuidbytes].showPercent;


                                Dispatcher.UIThread.InvokeAsync(() =>
                                {
                                    showRecvProgress.stackPanelParent.Children.Remove(showPercent.bar);
                                    showRecvProgress.stackPanelParent.Children.Remove(showPercent.percent);

                                });

                                uuidRecvDict.Remove(uuidbytes);
                                set.Remove(uuidbytes);


                            }
                            else
                            {
                                // MessageBox.Show("接收失败");
                                client.Send(SendHandle.SendResendPack(uuidbytes, uuidRecvDict[uuidbytes].packnum, uuidRecvDict[uuidbytes].hasRecvSize));
                            }
                            break;

                        default:
                            //如果set中存在uuidbytes ，猜测这可能是发送文件的时候出现的,催促重新发送

                            if (set.Count() == 0)
                            {
                                MessageBox.Show("不合法的请求头");
                                break;
                            }
                            else
                            {
                                //可能同时接收多个文件，发个包，催促下
                                foreach (byte[] uuid in set)
                                    client.Send(SendHandle.SendResendPack(uuid, uuidRecvDict[uuid].packOrder, uuidRecvDict[uuid].filesize));
                            }
                            break;

                    }



                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    if (!client.Connected)
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            parentPanel.Children.Remove(showRecvProgress.stackPanelParent);
                            stackPanelSocketDict.Remove(parentPanel);
                        });


                        //如果同时接收多个文件 ,set里面储存所有uuid 为 key 的接收文件任务，循环删除
                        foreach (byte[] uuidkey in set)
                        {
                            if (uuidRecvDict[uuidkey].recvSocket == client)
                            {
                                uuidRecvDict[uuidkey].stream.Close();
                                uuidRecvDict.Remove(uuidkey);
                            }
                        }

                        client.Close();
                    }
                    break;

                }
               
            }

        }
        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="sender"></param>
        public void SendText(Button sender)
        {
            StackPanel parent = sender.Parent as StackPanel;
            TextBox textBox = parent.FindByName<TextBox>("Content");
            if (textBox.Text == null||textBox.Text.Length==0)
            {
                MessageBox.Show("输入不能为空！请输入内容。");
               
            }else
            {
                byte[] data = SendHandle.AddTextInfoHeader(textBox.Text);
                stackPanelSocketDict[parent].SendAsync(data, SocketFlags.None);
            }

                
        }
      

        /// <summary>
        /// 显示接收文本
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="panel"></param>
        /// <param name="len"></param>
         void ShowContent(byte[] buf,TextBox textbox, int len)
        {
            Dispatcher.UIThread.Post(() =>
            {
                textbox.Text = RecvHandle.GetProcessedText(buf, len);
            });

        }
      
  
        public void ReSend()
        {

            foreach (byte[] uuid in uuidRecvDict.Keys)
            {
                byte[] resend= SendHandle.SendResendPack(uuid, uuidRecvDict[uuid].packnum, uuidRecvDict[uuid].hasRecvSize);
                uuidRecvDict[uuid].recvSocket.Send(resend);

            }

        }
        private void SendText(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            StackPanel parent = btn.Parent as StackPanel;
            TextBox textBox = parent.FindByName<TextBox>("Content");
            if (textBox.Text == null || textBox.Text.Length == 0)
            {
                MessageBox.Show("输入不能为空！请输入内容。");
            }
            else
            {
                byte[] data = SendHandle.AddTextInfoHeader(textBox.Text);
                stackPanelSocketDict[parent].SendAsync(data, SocketFlags.None);
            }
        }
    }
}
