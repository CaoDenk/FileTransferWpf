using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Dialogs
{
    internal class MyFiles
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
        public  string save(Window parent,string path)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialFileName = path;

            Task<string> filepath=    saveFileDialog.ShowAsync(parent);
            return filepath.Result;

        }


    }



     
   

}
