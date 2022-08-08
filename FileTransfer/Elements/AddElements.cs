using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;



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
        public static StackPanel AddElement(StackPanel panel, System.EventHandler<RoutedEventArgs> @event)
        {

            var res= Dispatcher.UIThread.InvokeAsync(() =>
            {
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

                TextBlock textBlock = new TextBlock();
                textBlock.Name = "ShowRecvText";
                
                stackPanel.Children.Add(textBlock);

                TextBox textBox = new TextBox();
                textBox.Name = "Content";
                textBox.Height = 24;
                textBox.AcceptsReturn = true;
                textBox.FontSize = 18;
                textBox.Margin= new Thickness(0, 0, 0, 5);
                stackPanel.Children.Add(textBox);


                Button sendText = new Button();
                sendText.Content = "发送";
                sendText.HorizontalAlignment = HorizontalAlignment.Center;
                sendText.Width = 200;
                sendText.Background = Brush.Parse("LightBlue");
                sendText.Click += @event;
                stackPanel.Children.Add(sendText);

                

               
                //ProgressBar progressBar = new ProgressBar();
                //progressBar.Minimum = 0;
                //progressBar.Maximum = 100;
                //progressBar.Name = "ProgressBar";

                


                panel.Children.Add(stackPanel);
                return stackPanel;

                });
            return res.Result;

            }



        public static ProgressBar AddProgressFromStackPanel(StackPanel panel)
        {
            var res= Dispatcher.UIThread.InvokeAsync<ProgressBar>(() => {


                    ProgressBar progressBar = new ProgressBar();
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;
                    progressBar.Height = 10;
                    progressBar.Margin= new Thickness(0, 5, 0, 0);
                    //TextBlock showProgress = new TextBlock();
                    //showProgress.HorizontalAlignment = HorizontalAlignment.Right;

                    //Binding mybinding = new Binding();
                    //mybinding.Source = progressBar;
                    //PropertyPath propertyPath = new PropertyPath(ProgressBar.ValueProperty);
                    //mybinding.Path = propertyPath;
                    //BindingOperations.SetBinding(showProgress, TextBlock.TextProperty, mybinding);

                    //panel.Children.Add(showProgress);
                    //panel.Children.Add(progressBar);
                    panel.Children.Add(progressBar);
                    return progressBar;
                
             
             
            });
            return res.Result;
        }
        public static void SetBarValue(ProgressBar bar, double value)
        {
            Dispatcher.UIThread.Post(() => {
                bar.Value = value;
            }
            );
        }
        public static void DeleteProgressBarAndTextBlock()
        {



        }


    }



}
