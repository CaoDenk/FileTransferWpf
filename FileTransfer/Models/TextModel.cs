using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal class TextModel
    {
        static byte[] TextToFileStream(string s) 
        {


            return null;
        }
        /*
         
         如果字符串内容比较多，将转成文件发送
        感觉有点多此一举，
        方案保留
         */


        public static byte[] AddInfoHeader(string s)
        {
            //if(s.Length>1020)
            //{
            //    return TextToFileStream(s);
            //}
            byte[] data = Encoding.Default.GetBytes(s);
            byte[] dest = new byte[data.Length+4];

            Array.Copy(data, 0, dest, 4, data.Length);
            return dest;
        }
    }
}
