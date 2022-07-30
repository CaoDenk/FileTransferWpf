using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Dialogs
{
    internal class ChooseFiles
    {

        //public ChooseFiles(Window parent)
        //{

        //}
        public string[] open(Window parent)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.AllowMultiple = true;
            Task<string[]> files = openFileDialog.ShowAsync(parent);
           
            return files.Result;
        }
    }
}
