using System;
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
            int lTRow=0, lTCol=0;
            int addRow = 1, addCol = 1;
           
            while (true)
            {
                int ltLeft = (int)leftTop.Dispatcher.Invoke(new Func<double>(() => 
                (double)leftTop.GetValue(Canvas.LeftProperty)));
                int ltTop = (int)leftTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.TopProperty)));

                int rtLeft = (int)rightTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.LeftProperty)));
                int rtTop = (int)rightTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.TopProperty)));

                int lbLeft = (int)leftBottom.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.LeftProperty)));
                int lbTop = (int)leftBottom.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.TopProperty)));
                int rbLeft = (int)leftTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.LeftProperty)));
                int rbTop = (int)leftTop.Dispatcher.Invoke(new Func<double>(() =>
                (double)leftTop.GetValue(Canvas.TopProperty)));
                Thread.Sleep(3);
                if (ltLeft<0|| ltLeft > length - 94)
                {
                    addRow = -addRow;
                }
                if (ltTop < 0.0 || ltTop > length - 94)
                {
                    addCol = -addCol;
                }
                lTRow += addRow;
                lTCol += addCol;
                leftTop.Dispatcher.Invoke(new Action(() =>
                {
                    leftTop.SetValue(Canvas.TopProperty, 0.058 * length + lTCol*0.8);
                    leftTop.SetValue(Canvas.LeftProperty, 0.058 * length + lTRow);
                }));

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
