using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using FileTransfer.Tools;
using FileTransfer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Models
{
    internal class NewDialogModel
    {

      /// <summary>
      /// 添加元素到指定容器，包括按钮，输入框
      /// </summary>
      /// <param name="panel"></param>
      /// <param name="event"></param>
      /// <returns></returns>
        public  StackPanel AddElement(StackPanel panel, EventHandler<RoutedEventArgs> @event)
        {


           Task<StackPanel> task= Dispatcher.UIThread.InvokeAsync<StackPanel>(() =>
            {
                StackPanel wrapeStackPanel = new StackPanel();
                WrapPanel wrapePanel = new WrapPanel();

                Button choose = new Button();
                choose.Content = "选择文件";
                wrapePanel.Children.Add(choose);

                Button sendFile = new Button();
                sendFile.Content = "发送文件";
                wrapePanel.Children.Add(sendFile);

                wrapeStackPanel.Children.Add(wrapePanel);

                TextBlock textBlock = new TextBlock();
                textBlock.Name = "ShowRecvText";          
                wrapeStackPanel.Children.Add(textBlock);

                TextBox textBox = new TextBox();
                textBox.Watermark = "input";
                textBox.Name = "Content";
                textBox.FontFamily = Avalonia.Media.FontFamily.Parse("Microsoft YaHei");
                wrapeStackPanel.Children.Add(textBox);

                Button sendText = new Button();
                sendText.Content = "发送";

                sendText.Click += @event;
                //Type t =SendText_Click
                wrapeStackPanel.Children.Add(sendText);

                panel.Children.Add(wrapeStackPanel);
                return wrapeStackPanel;
            });
            
            return task.Result;
        }

   

    }
}
