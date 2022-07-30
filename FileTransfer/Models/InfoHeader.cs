using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal class InfoHeader
    {
       public const int TEXT = 0;
       public const int FILE = 1;
       public const int DIR = 2;
       public const int CONTINUE_RECV = 10;//断点续传需要用到

    }
}
