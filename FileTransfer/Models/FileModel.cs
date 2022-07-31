using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal class FileModel
    {
        string fileName;
        ulong fileSize; //不可能出现超过int范围的文件，    
        //2^32 = 4G 一算好像还是真有可能的
        int offset=0;//偏移量，从信息头偏移到数据中
        FileModel fileModel;

      public FileModel(string fileName, ulong fileSize, FileModel fileModel)
        {
            this.fileName = fileName;
            this.fileSize = fileSize;
            this.fileModel = fileModel;
        }

        public  byte[] AddInfoHeader(string s)
        {
            //if(s.Length>1020)
            //{
            //    return TextToFileStream(s);
            //}
            byte[] data = Encoding.Default.GetBytes(s);
            byte[] dest = new byte[data.Length + 4];

            Array.Copy(data, 0, dest, 4, data.Length);
            return dest;
        }
         byte[] InfoHeaderToByte()
        {
            StringBuilder sb = new StringBuilder();           
            string s=  string.Format("{filename={0};filesize={1}}", fileName,fileSize);
            return Encoding.Default.GetBytes(s);
        }
    }
}
