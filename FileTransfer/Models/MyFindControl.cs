using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal static class MyFindControl
    {

        public  static T? FindByName<T >(this StackPanel panel,string Name) where T : class, IControl
        {
            foreach(IControl c in panel.Children)
            {
                if (c.Name == Name)
                    return (T)c;
            }
            return null;
        }
        public static T? FindByName<T>(this WrapPanel panel, string Name) where T : class, IControl
        {
            foreach (IControl c in panel.Children)
            {
                if (c.Name == Name)
                    return (T)c;
            }
            return null;
        }


    }
}
