using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using FileTransfer.Models;

namespace FileTransfer.Elements
{
    internal class AddElements
    {
        
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        public static ShowRecvProgress AddElement(StackPanel panel, System.EventHandler<RoutedEventArgs> @event)
        {

            var res= Dispatcher.UIThread.InvokeAsync<ShowRecvProgress>(() =>
            {

                ShowRecvProgress showRecvProgress = new ShowRecvProgress();

                StackPanel stackPanel = new StackPanel();
                
                //WrapPanel wrapePanel = new WrapPanel();

                //Button choose = new Button();
                //choose.Content = "选择文件";
                //choose.Margin = new Thickness(0, 0, 5, 0);
                //choose.Background = Brushes.LightBlue;
                //wrapePanel.Children.Add(choose); 
            

                //Button sendFile = new Button();
                //sendFile.Content = "发送文件";
                //sendFile.Margin = new Thickness(0, 0, 0, 0);
                //sendFile.Background = Brushes.LightBlue;
                //wrapePanel.Children.Add(sendFile);


                //stackPanel.Children.Add(wrapePanel);
                //显示接收内容
                TextBox ShowRecvText = new TextBox();
                ShowRecvText.Name = "ShowRecvText";
                ShowRecvText.BorderThickness = new Thickness(0);
                ShowRecvText.IsReadOnly = true;
                ShowRecvText.Height = 40;
                ShowRecvText.FontFamily = "Microsoft YaHei";

                showRecvProgress.showRecvText = ShowRecvText;
                stackPanel.Children.Add(ShowRecvText);

                //输入框
                TextBox textBox = new TextBox();
                textBox.Name = "Content";
                textBox.Height = 40;
                textBox.AcceptsReturn = true;
                textBox.FontSize = 18;
                textBox.Margin= new Thickness(0, 0, 0, 5);
                textBox.Watermark = "input";
                textBox.FontFamily = "Microsoft YaHei";

                showRecvProgress.inputContent = textBox;
                stackPanel.Children.Add(textBox);


                Button sendText = new Button();
                sendText.Content = "发送";
                sendText.HorizontalAlignment = HorizontalAlignment.Center;
                sendText.Width = 200;
                sendText.Background = Brush.Parse("LightBlue");
                sendText.Click += @event;

                showRecvProgress.sendText = sendText;
                stackPanel.Children.Add(sendText);




                //ProgressBar progressBar = new ProgressBar();
                //progressBar.Minimum = 0;
                //progressBar.Maximum = 100;
                //progressBar.Name = "ProgressBar";



                showRecvProgress.stackPanelParent = stackPanel;

                panel.Children.Add(stackPanel);
                return showRecvProgress;

                });
            return res.Result;

            }



        public static ShowPercent AddProgressFromStackPanel(StackPanel panel)
        {
            var res= Dispatcher.UIThread.InvokeAsync<ShowPercent>(() => {

                    ShowPercent showPercent = new ShowPercent();

                    ProgressBar progressBar = new ProgressBar();
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;
                    progressBar.Height = 10;
                    progressBar.Margin= new Thickness(0, 5, 0, 0);

                    showPercent.bar = progressBar;
                    panel.Children.Add(progressBar);

                    TextBlock showProgress = new TextBlock();
                    showProgress.HorizontalAlignment = HorizontalAlignment.Right;

                    showPercent.percent = showProgress;
                   
                    panel.Children.Add(showProgress);    

                //Binding mybinding = new Binding();
                //mybinding.Source = progressBar;
                //PropertyPath propertyPath = new PropertyPath(ProgressBar.ValueProperty);
                //mybinding.Path = propertyPath;
                //BindingOperations.SetBinding(showProgress, TextBlock.TextProperty, mybinding);

                //panel.Children.Add(showProgress);
                //panel.Children.Add(progressBar);




              
                    return showPercent;
                
             
             
            });
            return res.Result;
        }
        public static void SetBarValue(ShowPercent percent , double value)
        {
            Dispatcher.UIThread.Post(() => {
                percent.bar.Value = value;
                percent.percent.Text = value.ToString();
            }
            );
        }
        public static void DeleteProgressBarAndTextBlock()
        {



        }


    }



}
