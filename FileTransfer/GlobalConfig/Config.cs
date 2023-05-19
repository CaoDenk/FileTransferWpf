﻿using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.GlobalConfig
{
    /// <summary>
    /// 全局配置
    /// </summary>
    internal class Config
    {
        public const int TEXT_BUFER_SIZE=1024*4;
        public const int FILE_BUFFER_SIZE=1024*32;
        public const int OFFSET = 16;
        public const int FULL_SIZE = FILE_BUFFER_SIZE - OFFSET-4;
    
    }
}
