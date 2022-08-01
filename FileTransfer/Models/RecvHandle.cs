using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using csharp_json;
namespace FileTransfer.Models
{
    internal class RecvHandle
    {

        byte[] data;
        int len;
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
        public string GetFileName()
        {
            Span<byte> span = new Span<byte>(data);
            int offset = BitConverter.ToInt32(span[4..]);

            //JsonObject jsonObject
            Json json = new Json();
            json.readFromBuf(data, offset);
            JsonObject jsonObject= (JsonObject)json.parse();
            return jsonObject.getString("filename");
             //"";
        }
        void FileHandle(string filepath,Socket client)
        {
            Json json = new Json();
            //json.readFromBuf()





        }

    }
}
