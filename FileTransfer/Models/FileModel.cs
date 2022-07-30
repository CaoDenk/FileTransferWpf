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
        //2^32 *8   32g 会存在超过32g的文件

        FileModel fileModel;




    }
}
