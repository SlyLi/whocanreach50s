using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace 谁能坚持50秒
{
    struct RectMsg
    {
        public Rectangle rect;
        public double initLeft;
        public double initTop;
        public double width;
        public double height;
        public double addRow;
        public double addCol;
        public double actualLeft;
        public double actualTop;

        public RectMsg(Rectangle _rect)
        {
            rect = _rect;
            initLeft = 0;
            initTop = 0;
            width = 0;
            height = 0;
            actualLeft = 0;
            actualTop = 0;
            addRow = 1;
            addCol = 1;
        }

    }

    public class MoveRect
    {
        private RectMsg[] Rects = new RectMsg[4];
        private Button Center;
        public bool StopMove { get; set; }
        public  bool GameOver { get; set; }
        private double Length { get; set; }
        private int initFlag=4;
        private Thread th;
        /// <summary>
        /// params length center,lefttop,righttop,leftbottom,rightbottom
        /// </summary>
        /// <param name="rects"></param>
        public MoveRect(double length, Button button,Rectangle[] rects )
        {
            StopMove = false;
            Length = length;
            Center = button;
            for (int i = 0; i < rects.Length; i++)
            {
                Rects[i] = new RectMsg(rects[i]);
            }
            Rects[1].addRow = -1;
            Rects[2].addCol = -1;
            Rects[3].addRow = -1;
            Rects[3].addCol = -1;
        }

        public void GameBeign()
        {
            GameOver = false;
            for (int i = 0; i < Rects.Length; i++)
            {
                if (initFlag-- > 0)
                {
                    Rects[i].initLeft = Rects[i].rect.Dispatcher.Invoke(new Func<double>(() =>
                        (double)Rects[i].rect.GetValue(Canvas.LeftProperty)));
                    Rects[i].initTop = Rects[i].rect.Dispatcher.Invoke(new Func<double>(() =>
                        (double)Rects[i].rect.GetValue(Canvas.TopProperty)));
                    Rects[i].width = Rects[i].rect.Dispatcher.Invoke(new Func<double>(() =>
                         Rects[i].rect.ActualWidth));
                    Rects[i].height = Rects[i].rect.Dispatcher.Invoke(new Func<double>(() =>
                          Rects[i].rect.ActualHeight));
                    //    initFlag = false;
                }

                Rects[i].actualLeft = Rects[i].initLeft;
                Rects[i].actualTop = Rects[i].initTop;
            }
            th = new Thread(StartMoving);
            th.Start();
        }


        private void StartMoving()
        {
            while (!GameOver)
            {
                while (StopMove) ;
                for (int i = 0; i < Rects.Length; i++)
                {
                    if (Rects[i].actualLeft < 0.0 || Rects[i].actualLeft > Length - Rects[i].width)
                    {
                        Rects[i].addRow = -Rects[i].addRow;
                    }
                    if (Rects[i].actualTop < 0.0 || Rects[i].actualTop > Length - Rects[i].height)
                    {
                        Rects[i].addCol = -Rects[i].addCol;
                    }

                    Rects[i].actualLeft += Rects[i].addRow;
                    Rects[i].actualTop += 0.8 * Rects[i].addCol;

                    Rects[i].rect.Dispatcher.Invoke(new Action(() =>
                    {
                        Rects[i].rect.SetValue(Canvas.LeftProperty, Rects[i].actualLeft);
                        Rects[i].rect.SetValue(Canvas.TopProperty, Rects[i].actualTop);
                    }));
                    if (Judgement(Rects[i]))
                        GameOver = true;
                    Thread.Sleep(3);
                }
            }
        }

        private bool Judgement(RectMsg rect)
        {
            double centerLeft, centerTop, centerRight, centerBottom;

            centerLeft = Center.Dispatcher.Invoke(new Func<double>(() =>
                (double)Center.GetValue(Canvas.LeftProperty)));
            centerTop = Center.Dispatcher.Invoke(new Func<double>(() =>
                (double)Center.GetValue(Canvas.TopProperty)));
            centerRight = Center.Dispatcher.Invoke(new Func<double>(() =>
                  Center.ActualWidth)) + centerLeft;
            centerBottom = Center.Dispatcher.Invoke(new Func<double>(() =>
               Center.ActualHeight)) + centerTop;

            double minLeft = rect.actualLeft > centerLeft ? rect.actualLeft : centerLeft;
            double minTop = rect.actualTop > centerTop ? rect.actualTop : centerTop;

            double maxRight = 
                rect.actualLeft + rect.width > centerRight ? centerRight : rect.actualLeft + rect.width;
            double maxBottom = 
                rect.actualTop > centerBottom ? centerBottom : rect.actualTop + rect.height;

            return ((minLeft < maxRight) && (minTop < maxBottom));
        }



        private void MoveOne(RectMsg rectMsg)
        {

            if (rectMsg.actualLeft < 0.0 || rectMsg.actualLeft > Length -rectMsg.width)
            {
                rectMsg.addRow = -rectMsg.addRow;
            }
            if (rectMsg.actualTop < 0.0 || rectMsg.actualTop > Length - rectMsg.height)
            {
                rectMsg.addCol = -rectMsg.addCol;
            }

            rectMsg.actualLeft += rectMsg.addRow;
            rectMsg.actualTop += 0.8*rectMsg.addCol;
            rectMsg.rect.Dispatcher.Invoke(new Action(() =>
            {
                rectMsg.rect.SetValue(Canvas.LeftProperty, rectMsg.actualLeft);
                rectMsg.rect.SetValue(Canvas.TopProperty, rectMsg.actualTop);
            }));
        }

        public void Restart()
        {
            StopMove = false;
            GameOver = true;
            while (th.IsAlive) ;
            InitRect();
            GameBeign();
        }


        private void InitRect()
        {
            Rects[1].addRow = -1;
            Rects[2].addCol = -1;
            Rects[3].addRow = -1;
            Rects[3].addCol = -1;

            for (int i = 0; i < Rects.Length; i++)
            {
                Rects[i].rect.Dispatcher.Invoke(new Action(() =>
                {
                    Rects[i].rect.SetValue(Canvas.LeftProperty, Rects[i].initLeft);
                    Rects[i].rect.SetValue(Canvas.TopProperty, Rects[i].initTop);
                }));
            }

        }

    }
}
