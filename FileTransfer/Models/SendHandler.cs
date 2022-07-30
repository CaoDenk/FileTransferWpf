using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    internal class SendHandler
    {



    }
}
