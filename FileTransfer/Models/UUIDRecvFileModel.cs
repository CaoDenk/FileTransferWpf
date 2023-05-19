﻿using Avalonia.Controls;
using FileTransfer.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal class UUIDRecvFileModel
    {
        public FileStream stream;
        public long filesize;
        public int packnum;
        //public List<int> verifyPack=new List<int>();
        public DateTime start;
        public long hasRecvSize=0;
        public int packOrder=0;

        //public ProgressBar progressBar;
        public int errorpack = 0;
        public ShowPercent showPercent;

        /// <summary>
        /// 强制关闭需要
        /// </summary>
        public Socket recvSocket;
    }
}
