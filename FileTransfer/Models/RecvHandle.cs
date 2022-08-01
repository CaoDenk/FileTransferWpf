using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using csharp_json;
using FileTransfer.Tools;

namespace FileTransfer.Models
{
    internal class RecvHandle
    {

        byte[] data;
        int len;
        int offset;
        string fileName;
        public RecvHandle(byte[] data,int len)
        {
            this.data = data;
            this.len = len;
        }

        public int GetDataType()
        {

            return BitConverter.ToInt32(data);

        }
        public  string GetProcessedText()
        {
            return Encoding.Default.GetString(data,4,len-4);
        }
        /// <summary>
        /// 留下个小问题，会有拷贝么？
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            offset = BitConverter.ToInt32(data[4..]);

            //JsonObject jsonObject
            Json json = new Json();
            json.readFromBuf(data, 8,offset);
            JsonObject jsonObject= (JsonObject)json.parse();
            fileName= jsonObject.getString("filename");
            return fileName;
             //"";
        }
       public void FileHandle(string filepath,Socket client,int len)
        {
            //Json json = new Json();
            //json.readFromBuf()
            FileStream fileStream = File.Create(fileName);

            fileStream.Write(data,offset+8,len-offset-8);
            if(len<1024*1024*4)
            {
                fileStream.Close();
                Dispatcher.UIThread.Post(() =>
                {
                    MyMessage.show("info", "saved");
                });
            
                return;
            }
            // client.ReceiveAsync(data,SocketFlags.None);
            //while ((len=)>0)
            //{
            //    fileStream.Write(data, 0, len);
            //    if (len<1024*1024*4)
            //    {
            //        fileStream.Close();
            //        Dispatcher.UIThread.Post(() =>
            //        {
            //            MyMessage.show("info", "saved");
            //        });
            //        return ;
            //    }
            
            //}
       

        }

    }
}
