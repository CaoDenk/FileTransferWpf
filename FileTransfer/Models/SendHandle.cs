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
    
    offset=, 方便
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
        public static byte[] AddTextInfoHeader(string s)
        {
            byte[] data = Encoding.Default.GetBytes(s);
            byte[] dest = new byte[data.Length +4];
            Array.Copy(data, 0, dest, 4, data.Length);
            return dest;
        }
        /// <summary>
        /// 文件添加文件信息头
        /// buf的偏移量是绝对偏移量，直接偏移到数据所在位置，发送文件的时候顺序执行，将来断点续传的时候改用异步,加一个信息头，还得算偏移量，先发一个文件包，
        /// </summary>
        /// <returns></returns>  
        public void FileHandle(string filepath,Socket client)
        {
            FileStream fileStream = File.OpenRead(filepath);
            FileInfo fileInfo = new FileInfo(filepath);
            int bufsize = 1024 * 1024 * 4;
            byte[] buf = new byte[bufsize];
            Span<byte> buffer = new Span<byte>(buf);
            BitConverter.TryWriteBytes(buffer, 1);
            JsonObject jsonObject = new JsonObject();
            jsonObject.put("filename",fileInfo.Name);
            jsonObject.put("filesize",fileInfo.Length);
           
            byte[] infobyte=Encoding.Default.GetBytes(jsonObject.ToString());
            BitConverter.TryWriteBytes(buffer[4..], infobyte.Length);
            Array.Copy(infobyte,0, buf, 8, infobyte.Length);//第一个int是数据类型，第二个int是信息头偏移的地址
            //fileStream.Read
            int offset = infobyte.Length + 8;




            int readLenActually = 0;//= fileStream.Read(buf, offset, bufsize - offset);//发送文件应该阻塞下，要不然和文本发送混在一起
            //client.Send(buf);//这必须是同步
            
            client.Send(buf);
            if(readLenActually+offset<bufsize)
            {

            }
            //string str = Encoding.Default.GetString(buf);
            ////Console.Write(Encoding.Default.GetString(buf));
            while ((readLenActually = fileStream.Read(buf)) > 0)
            {
                Span<byte> span = new Span<byte>(buf);
                client.Send(span[0..readLenActually], SocketFlags.None);
            }


        }

     

        /// <summary>
        /// 添加文件信息头并发送
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        public void AddFileInfoHeaderAndSendFile(Socket client,string filepath,byte[] buf)
        {
            FileStream fileStream = File.OpenRead(filepath);
            FileInfo fileInfo = new FileInfo(filepath);

            //往前4字节（int）写入1
            Span<byte> buffer = new Span<byte>(buf);
            BitConverter.TryWriteBytes(buffer, InfoHeader.FILE);
            
            JsonObject jsonObject = new JsonObject();
            jsonObject.put("filename", fileInfo.Name);
            jsonObject.put("filesize", fileInfo.Length);
            string jsStr=jsonObject.ToString();
            byte[] infobyte = Encoding.Default.GetBytes(jsStr);

            Array.Copy(infobyte,0, buf, 8, infobyte.Length);
            BitConverter.TryWriteBytes(buffer[4..], infobyte.Length);
            client.Send(buf);

            byte[] fileBuf =new byte[Config.FileBufSize];//
            int offset = 16;
            int len;
            //int totalSize = 0;
            int packageOrder = 0;
            while (true)
            {         
                len=   fileStream.Read(fileBuf, offset, fileBuf.Length-offset);
                //fileStream.Flush();
                //totalSize += len;
                if (len==Config.FileBufSize-offset)
                {
                    AddFileType(fileBuf, InfoHeader.CONTINUE_RECV, 0);
                    AddFileType(fileBuf, packageOrder, 4);
                    client.Send(fileBuf);
                }
                else   //最后一次加上结束标志
                {
                    AddFileType(fileBuf, InfoHeader.SEND_FINISHED, 0);                
                    AddFileType(fileBuf, packageOrder, 4);
                    AddFileType(fileBuf, len, 8);
                    client.Send(fileBuf);
                    
                    fileStream.Close();
                    break;
                }
                packageOrder++;      
            }

        


        }
        public void AddFileType(byte[] buf,int type,int offset)
        {

            Span<byte> span = new Span<byte>(buf);
            BitConverter.TryWriteBytes(span.Slice(offset), type);
        }


    }
}
