using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace 谁能坚持50秒
{
    public class Clock
    {
        Timer timer;
        Label timerbox;
        decimal currentCount;

        public Clock(Label box)
        {
            currentCount = 0;
            timerbox = box;
            InitTimer();
        }

        private void InitTimer()
        {
            //设置定时间隔(毫秒为单位)
            int interval = 100;
            timer = new Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer.Enabled = true;
            //绑定Elapsed事件
            timer.Elapsed += new ElapsedEventHandler(TimerUp);
        }
        private void TimerUp(object sender,ElapsedEventArgs e)
        {
            try
            {
                currentCount += 0.1m;
                timerbox.Dispatcher.Invoke(new Action(() => {
                    timerbox.Content = currentCount;
                }));
            }
            catch (Exception)
            {

               // throw;
            }
        }

        public void Pause()
        {
            timer.Enabled = false;
        }

        public void Continue()
        {
            timer.Enabled = true;
        }

        public decimal Over()
        {
            timer.Enabled = false;
            return currentCount;

        }

        public void Restart()
        {
            timer.Enabled = false;
            currentCount = 0m;
        }
        

    }
}
