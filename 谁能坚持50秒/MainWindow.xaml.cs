﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace 谁能坚持50秒
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MoveRect moving ;
        Clock clock;
        readonly double length = 550;
        bool enableMove = false;
        bool enableBegin = true;
        double spanLeft = 0;
        double spanTop = 0;
        double initX = 0;
        double initY = 0;
        public MainWindow()
        {
           
            InitializeComponent();
            clock = new Clock(timer);
            clock.Pause();
            moving = new MoveRect( clock, length, center,
                new Rectangle[] { leftTop, rightTop, leftBottom, rightBottom });
            moving.GameOver =false;
           
          
        }

        public new void Closed(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
       
        //鼠标移动
        private void Center_MouseMove(object sender, MouseEventArgs e)
        {
            if (enableMove&&(!moving.GameOver))
            {
                if (choice.Content.ToString() == "继续")
                {
                    moving.StopMove = true;
                    clock.Pause();
                    choice.Content = "暂停";
                }
                moving.StopMove = false;
                var cLeft = e.GetPosition(canvas).X - spanLeft;
                var cTop = e.GetPosition(canvas).Y - spanTop;
                if(enableBegin)
                {
                    enableBegin = false;
                    clock.Continue();
                    moving.GameBeign();
                    initX = cLeft;
                    initY = cTop;
                    
                }
                if(!moving.GameOver)
                    clock.Continue();
                
                //设置矩形的位置
                if(cTop>0&&cTop<length-72)
                {
                    center.SetValue(Canvas.TopProperty, cTop); 
                }
                if(cLeft>0&&cLeft<length-72)
                {
                    center.SetValue(Canvas.LeftProperty, cLeft);
                }

            }
           
        }
        //鼠标松开
        private void Center_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //释放鼠标捕获
            center.ReleaseMouseCapture();
            enableMove = false;
            spanLeft = spanTop = 0;
        }
        //鼠标按下
        private void Center_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //创建鼠标捕获
            double i = canvas.ActualWidth;
            Mouse.Capture(center);
            enableMove = true;
            spanLeft = e.GetPosition(canvas).X - Canvas.GetLeft(center);
            spanTop = e.GetPosition(canvas).Y - Canvas.GetTop(center);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if(button.Content.ToString()=="暂停")
            {
                moving.StopMove = true;
                clock.Pause();
                button.Content = "继续";
            }
            else
            {
                if (!moving.GameOver)
                    clock.Continue();
                moving.StopMove = false;
                button.Content = "暂停";
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            moving.Restart();
            moving.StopMove = true;
            clock.Restart();
            timer.Content = 0;
            center.SetValue(Canvas.TopProperty, initY);
            center.SetValue(Canvas.LeftProperty, initX);
        }

    }
}
