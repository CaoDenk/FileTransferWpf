using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.Views
{
    internal interface ChatDialog
    {
       void ShowMsg(string msg, Color color, LayoutOptions options);

    }
}
