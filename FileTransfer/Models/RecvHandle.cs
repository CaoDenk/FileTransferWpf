using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal class RecvHandle
    {
        public FileType recv(byte[] data)
        {
            int info =BitConverter.ToInt32(data, 0);

            switch(info)
            {
                case 0:
                    return FileType.TEXT;
                case 1:
                    return FileType.FILE;
                default:
                    throw new Exception("error in file infoheader!");
                    
            }


        }


    }
}
