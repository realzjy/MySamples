using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程编程
{
    class TimerManager
    {
        public void Run()
        {
            Console.WriteLine("run begin");

            System.Timers.Timer t = new System.Timers.Timer(3000);   //实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += //达到间隔时发生
                (s, e) =>
                    {
                        Console.WriteLine("OK!" + e.SignalTime);
                    };
            //new ElapsedEventHandler(theout); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

            Console.WriteLine("run finish");
        }

        private static int i = 0;

        public void TimerTest()
        {
            Timer t = new Timer(
                (s) => { Console.WriteLine(i); i++; },
                null,
                Timeout.InfiniteTimeSpan,
                Timeout.InfiniteTimeSpan
                );
            t.Change(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));            
        }
    }
}
