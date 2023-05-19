using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace FileTransfer.Tools
{
    internal static class FindControl
    {
        public static T? FindByName<T>(this StackPanel panel, string name) where T :class, IControl
        {
            
            foreach(IControl c in panel.Children)
            {
                if (c.Name == name)
                    return (T)c;
            }
            return null;
        }


    }
}
