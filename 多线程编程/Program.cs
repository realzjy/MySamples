using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程编程
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("write start");

            Console.OutputEncoding = Encoding.Unicode;

            string strcode = "我是小明";
            byte[] buffer = Encoding.UTF8.GetBytes(strcode);
            string msg = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.WriteLine(msg);

            Console.WriteLine(ASCIIEncoding.UTF8 +  "中文输出");

            //Console.WriteLine(Thread.CurrentThread.GetHashCode());
            //Console.OutputEncoding = Encoding.Unicode;
            //for (int i = 0; i < 5; i++)
            //{
            //    CreateThread.OneThread();
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    CreateThread.Pool("number" + i.ToString());
            //}

            //CreateThread.OneTask();

            //CreateThread.RunInvoke();

            //CreateThread.IsLock = true;
            //for (int i = 0; i < 3; i++)
            //{
            //    Thread t = new Thread(new ThreadStart(() => { CreateThread.RunLock(); }));
            //    t.Start();
            //}
            //for (int i = 0; i < 3; i++)
            //{
            //    Thread t = new Thread(new ThreadStart(()=> { CreateThread.RunMonitor(); }));
            //    t.Start();
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    CreateThread.RunXXXResetEvent();
            //}

            //CreateThread.RunThread();

            //TaskManager manager = new TaskManager();
            //manager.Run();

            //TimerManager manager = new TimerManager();
            //manager.Run();
            //manager.TimerTest();

            Console.ReadLine();
        }
    }
}
