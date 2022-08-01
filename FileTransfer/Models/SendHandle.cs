using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using csharp_json;
namespace FileTransfer.Models
{

    /**
     传输加上信息头
     前四字节表示 第一个 
    00 代表文本 02代表文件 03代表文件夹  

     properties表示
    offset=,
    filename= ,
    filesize=
     */
    
    internal class SendHandle
    {
    
        string fileName;
        ulong fileSize; //不可能出现超过int范围的文件，    
        //2^32 = 4G 一算好像还是真有可能的
        int offset = 0;//偏移量，从信息头偏移到数据中
        //DataTypeModel fileModel;

        /// <summary>
        /// 添加信息头
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] AddInfoHeader(string s)
        {
            byte[] data = Encoding.Default.GetBytes(s);
            byte[] dest = new byte[data.Length +4];
            Array.Copy(data, 0, dest, 4, data.Length);
            return dest;
        }
        /// <summary>
        /// 文件添加文件信息头
        /// </summary>
        /// <returns></returns>
        //byte[] InfoHeaderToByte()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string s = string.Format("{filename={0};filesize={1}}", fileName, fileSize);
        //    return Encoding.Default.GetBytes(s);
        //}

        public void FileHandle(string filepath,Socket client)
        {
            FileStream fileStream = File.OpenRead(filepath);
            FileInfo fileInfo = new FileInfo(filepath);
            int bufsize = 1024 * 1024 * 4;
            byte[] buf = new byte[bufsize];
            Span<byte> buffer = new Span<byte>(buf);
            BitConverter.TryWriteBytes(buffer, 1);
            JsonObject jsonObject = new JsonObject();
            string s = string.Format("{\"filename\"=\"{0}\",\"filesize=\"{1}\"}", fileInfo.Name, fileInfo.Length);
            byte[] infobyte=Encoding.Default.GetBytes(s);
            BitConverter.TryWriteBytes(buffer[4..], infobyte.Length);
            Array.Copy(infobyte,0, buf, 8, infobyte.Length);//第一个int是数据类型，第二个int是信息头偏移的地址
            //fileStream.Read
            int offset = infobyte.Length + 8;
            int readLenActually= fileStream.Read(buf, offset, bufsize - 8);//发送文件应该阻塞下，要不然和文本发送混在一起
            client.Send(buf);//这必须是同步

            while((readLenActually=fileStream.Read(buf))>0)
            {
                client.Send(buf,readLenActually,SocketFlags.None);
            }
            

        }

    }
}
