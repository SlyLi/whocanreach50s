﻿using System;
using System.Collections.Generic;
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
        Thread tMoveRect;
        public delegate void delegate1(ref int lTLeft, ref int lTRight,
            ref int lTTop, ref int lTBottom);
        public MainWindow()
        {
            InitializeComponent();
            InitRectPosition();
            tMoveRect = new Thread(MoveRectangle)
            {
                IsBackground = true
            };
            tMoveRect.Start();
            //length = (int)canvas.ActualHeight;
        }
        readonly int length =550;
        bool enableMove = false;
        double spanLeft = 0;
        double spanTop = 0;
        //鼠标移动
        private void Center_MouseMove(object sender, MouseEventArgs e)
        {
            if (enableMove)
            {
                var cLeft = e.GetPosition(canvas).X - spanLeft;
                var cTop = e.GetPosition(canvas).Y - spanTop;
                //设置矩形的位置
                if(cTop>0&&cTop<length-72)
                {
                    center.SetValue(Canvas.TopProperty, cTop); 
                }
                if(cLeft>0&&cLeft<length-72)
                {
                    center.SetValue(Canvas.LeftProperty, cLeft);
                }
               // Canvas.SetLeft(center, cLeft);
               // Canvas.SetTop(center, cTop);
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
            Mouse.Capture(center);
            enableMove = true;
            spanLeft = e.GetPosition(canvas).X - Canvas.GetLeft(center);
            spanTop = e.GetPosition(canvas).Y - Canvas.GetTop(center);
        }

        private  void  DelegatelGetPosition(ref int lTLeft , ref int lTRight ,
            ref int lTTop , ref int lTBottom )
        {
            lTLeft = (int)leftTop.GetValue(Canvas.LeftProperty);
            lTRight = (int)leftTop.GetValue(Canvas.RightProperty);
            lTTop = (int)leftTop.GetValue(Canvas.TopProperty);
            lTBottom = (int)leftTop.GetValue(Canvas.BottomProperty);
        }

        private void MoveRectangle()
        {
            int ltRow = 0, ltCol = 0, rtRow = 0, rtCol = 0, 
                lbRow = 0, lbCol = 0, rbRow = 0, rbCol = 0;
            int ltAddRow = 1, ltAddCol = 1, rtAddRow = 1, rtAddCol = 1,
                lbAddRow = 1, lbAddCol = 1, rbAddRow = 1, rbAddCol = 1;
           
            while (true)
            {
                int ltLeft = (int)leftTop.Dispatcher.Invoke(new Func<double>(() => 
                (double)leftTop.GetValue(Canvas.LeftProperty)));
                int ltTop = (int)leftTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.TopProperty)));
                if (ltLeft < 0 || ltLeft > length - 94)
                {
                    ltAddRow = -ltAddRow;
                }
                if (ltTop < 0 || ltTop > length - 94)
                {
                    ltAddCol = -ltAddCol;
                }
                ltRow += ltAddRow;
                ltCol += ltAddCol;
                leftTop.Dispatcher.Invoke(new Action(() =>
                {
                    leftTop.SetValue(Canvas.TopProperty, 0.058 * length + ltCol * 0.8);
                    leftTop.SetValue(Canvas.LeftProperty, 0.058 * length + ltRow);
                }));


                int rtLeft = (int)rightTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)rightTop.GetValue(Canvas.LeftProperty)));
                int rtTop = (int)rightTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)rightTop.GetValue(Canvas.TopProperty)));
                if (rtLeft < 0 || rtLeft > length-94)
                {
                    rtAddRow = -rtAddRow;
                }
                if (rtTop <0 || rtTop > length -77)
                {
                    rtAddCol = -rtAddCol;
                }
                rtRow += rtAddRow;
                rtCol += rtAddCol;
                rightTop.Dispatcher.Invoke(new Action(() =>
                {
                    rightTop.SetValue(Canvas.TopProperty, 0.029 * length + rtCol * 0.8);
                    rightTop.SetValue(Canvas.LeftProperty, 0.66 * length - rtRow);
                }));


                int lbLeft = (int)leftBottom.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftBottom.GetValue(Canvas.LeftProperty)));
                int lbTop = (int)leftBottom.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftBottom.GetValue(Canvas.TopProperty)));
                if (lbLeft < 0 || lbLeft > length-47)
                {
                    lbAddRow = -lbAddRow;
                }
                if (lbTop < 0 || lbTop > length -94)
                {
                    lbAddCol = -lbAddCol;
                }
                lbRow += lbAddRow;
                lbCol += lbAddCol;
                leftBottom.Dispatcher.Invoke(new Action(() =>
                {
                    leftBottom.SetValue(Canvas.TopProperty, 0.772 * length - lbCol * 0.8);
                    leftBottom.SetValue(Canvas.LeftProperty, 0.058 * length + lbRow);
                }));

                int rbLeft = (int)rightBottom.Dispatcher.Invoke(new Func<double>(() =>
                (double)rightBottom.GetValue(Canvas.LeftProperty)));
                int rbTop = (int)leftTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)rightBottom.GetValue(Canvas.TopProperty)));
                if (rbLeft < 0 || rbLeft > length - 158)
                {
                    rbAddRow = -rbAddRow;
                }
                if (rbTop < 0|| rbTop > length - 30)
                {
                    rbAddCol = -rbAddCol;
                }
                rbRow += rbAddRow;
                rbCol += rbAddCol;
                rightBottom.Dispatcher.Invoke(new Action(() =>
                {
                    rightBottom.SetValue(Canvas.TopProperty, 0.803 * length - rbCol * 0.8);
                    rightBottom.SetValue(Canvas.LeftProperty, 0.713 * length + rbRow);
                }));
                Thread.Sleep(1);
            }
        }

        private void InitRectPosition()
        {
            leftTop.SetValue(Canvas.TopProperty, 0.058 * length);
            leftTop.SetValue(Canvas.LeftProperty, 0.058 * length);

            rightTop.SetValue(Canvas.TopProperty, 0.029 * length);
            rightTop.SetValue(Canvas.LeftProperty, 0.66 * length);

            center.SetValue(Canvas.TopProperty, 0.445 * length);
            center.SetValue(Canvas.LeftProperty, 0.445 * length);

            leftBottom.SetValue(Canvas.TopProperty, 0.772 * length);
            leftBottom.SetValue(Canvas.LeftProperty, 0.058 * length);

            rightBottom.SetValue(Canvas.TopProperty, 0.803 * length);
            rightBottom.SetValue(Canvas.LeftProperty, 0.713 * length);
        }

    }
}
