using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程编程
{
    class CreateThread
    {
        /// <summary>
        /// 线程的使用
        /// 线程函数通过委托传递，可以不带参数，也可以带参数（只能有一个参数），可以用一个类或结构体封装参数。
        /// </summary>
        public static void OneThread()
        {
            Thread t = new Thread(new ThreadStart
                      (() => { Console.WriteLine("thread operation"); }));
            Console.WriteLine(t.GetHashCode() + "thread start");
            t.Start();
        }

        /// <summary>
        /// 线程池
        /// 由于线程的创建和销毁需要耗费一定的开销，过多的使用线程会造成内存资源的浪费，出于对性能的考虑，于是引入了线程池的概念。
        /// 线程池维护一个请求队列，线程池的代码从队列提取任务，然后委派给线程池的一个线程执行，线程执行完不会被立即销毁，这样既可
        /// 以在后台执行任务，又可以减少线程创建和销毁所带来的开销。
        /// 线程池线程默认为后台线程（IsBackground）。
        /// </summary>
        /// <param name="str"></param>
        public static void Pool(string str)
        {
            ThreadPool.QueueUserWorkItem(Write, str);
        }
        public static void Write(object str)
        { Console.WriteLine(str.ToString()); }

        /// <summary>
        /// Task类
        /// 使用ThreadPool的QueueUserWorkItem()方法发起一次异步的线程执行很简单，但是该方法最大的问题是没有一个内建的机制让你知道操作
        /// 什么时候完成，有没有一个内建的机制在操作完成后获得一个返回值。为此，可以使用System.Threading.Tasks中的Task类。
        /// 构造一个Task<TResult> 对象，并为泛型TResult参数传递一个操作的返回类型。
        /// </summary>
        public static void OneTask()
        {
            //Task<Int32> t = new Task<Int32>(n => Sum((Int32)n), 100);
            //t.Start();
            ////t.Wait();
            //Task cwt = t.ContinueWith(task => Console.WriteLine("The result is {0}", t.Result));

            Task<string> t2 = new Task<string>((o) => { o = o + "A"; return o.ToString(); }, "s");
            Task r2 = t2.ContinueWith<string>((o) => { Console.WriteLine(t2.Result); return t2.Result.ToString(); });
            t2.Start();
            bool b = false;
            while (!b)
            {
                b = r2.IsCompleted;
                Console.WriteLine("last result:" + b.ToString());
            }
        }
        //private static Int32 Sum(Int32 n)
        //{
        //    Int32 sum = 0;
        //    for (; n > 0; --n)
        //        checked { sum += n; } //结果溢出，抛出异常
        //    return sum;
        //}

        /// <summary>
        /// 委托异步执行
        /// 委托的异步调用：BeginInvoke() 和 EndInvoke()
        /// </summary>
        public static void RunInvoke()
        {
            MyDelegate md = 
                new MyDelegate(
                    //线程函数
                    (o) => { return o + "S"; });

            IAsyncResult result =
                md.BeginInvoke(
                    "Thread Param",
                    //异步回调函数
                    (o) => { Console.WriteLine(o.AsyncState); },
                    "Callback Param");

            //异步执行完成
            string resultstr = md.EndInvoke(result);
        }
        public delegate string MyDelegate(string data);

        /// <summary>
        /// 语法糖（Syntactic sugar），也译为糖衣语法，是由英国计算机科学家彼得·约翰·兰达（Peter J. Landin）
        /// 发明的一个术语，指计算机语言中添加的某种语法，这种语法对语言的功能并没有影响，但是更方便程序员使用。
        /// 通常来说使用语法糖能够增加程序的可读性，从而减少程序代码出错的机会。
        /// 
        /// Lock关键字
        /// 1.Lock关键字实际上是一个语法糖，它将Monitor对象进行封装，给object加上一个互斥锁，A进程进入此代码段时，
        /// 会给object对象加上互斥锁，此时其他B进程进入此代码段时检查object对象是否有锁？如果有锁则继续等待A进程
        /// 运行完该代码段并且解锁object对象之后，B进程才能够获取object对象为其加上锁，访问代码段。
        /// 2.Lock关键字封装的Monitor对象结构如下：
        /// 3.锁定的对象应该声明为private static object obj = new object(); 尽量别用公共变量和字符串、this、值类型。
        /// Monitor和Lock的区别
        /// 1.Lock是Monitor的语法糖。
        /// 2.Lock只能针对引用类型加锁。
        /// 3.Monitor能够对值类型进行加锁，实质上是Monitor.Enter(object) 时对值类型装箱。
        /// 4.Monitor还有其他的一些功能。
        /// </summary>
        public static void RunLock()
        {
            if (IsLock)
            {
                lock (_lock)
                {
                    Console.WriteLine("RunLock begin");
                    Thread.Sleep(1000);
                    Console.WriteLine("RunLock end");
                }
            }
            else
            {
                Console.WriteLine("RunLock begin");
                Thread.Sleep(1000);
                Console.WriteLine("RunLock end");
            }
        }
        public static void RunMonitor()
        {
            if (IsLock) { Monitor.Enter(_lock); }
            Console.WriteLine("RunMonitor begin");
            Thread.Sleep(1000);
            Console.WriteLine("RunMonitor end");
            if (IsLock) { Monitor.Exit(_lock); }
        }
        public static bool IsLock = false;
        private static object _lock = new object();

        public static void RunReaderWriterLock()
        {
            //ReaderWriterLock l = new ReaderWriterLock();
        }

        public static void RunXXXResetEvent()
        {
            AutoResetEvent arEvent = new AutoResetEvent(false);

            ThreadStart ts =
                new ThreadStart(
                        () =>
                        {
                            XXXi++;
                            bool b = arEvent.WaitOne();
                            Console.WriteLine("AutoResetEvent begin" + XXXi + b);
                            Thread.Sleep(1000);
                            Console.WriteLine("AutoResetEvent end" + XXXi);
                            arEvent.Set();
                        });

            Thread t = new Thread(ts);
            t.Start();
            arEvent.Set();
        }
        private static int XXXi = 0;

        public static void RunThread()
        {
            //Console.WriteLine("starting...");
            //Thread t = new Thread(
            //    () => { for (int i = 0; i < 50; i++) { Console.WriteLine("Asynchronous output"); } });
            //t.Start();
            //for (int i = 0; i < 50; i++) { Console.WriteLine("Synchronous output"); }
            ////t.Join(TimeSpan.FromSeconds(10));
            //Console.WriteLine("finish");

            //Console.WriteLine("starting...");
            //Thread t = new Thread(
            //    () => {
            //        for (int i = 0; i < 500; i++)
            //        { Console.WriteLine("Asynchronous output " + i); }
            //    });
            //Console.WriteLine(t.ThreadState.ToString());
            //t.Start();
            //Console.WriteLine(t.ThreadState.ToString());
            //Thread.Sleep(TimeSpan.FromMilliseconds(60));
            //Console.WriteLine(t.ThreadState.ToString());
            //t.Abort();
            //Console.WriteLine(t.ThreadState.ToString());
            //Console.WriteLine("finish");

            Thread t = new Thread(() => { OutPutOne("T1"); });
            Thread t2 = new Thread(() => { OutPutOne("T2"); });
            t.Priority = ThreadPriority.Lowest;
            t2.Priority = ThreadPriority.AboveNormal;
            t2.IsBackground = true;
            t.Start();
            t2.Start();
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

        }

        private static void OutPutOne(string str)
        {
            lock (_lock)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(str); Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }
            }
        }
    }

    /*
     C#中的AutoResetEvent和ManualResetEvent用于实现线程同步。其基本工作原理是多个线程持有同一个XXXResetEvent，
     在这个XXXResetEvent未被set前，各线程都在WaitOne()除挂起；在这个XXXResetEvent被set后，所有被挂起的线程中
     有一个（AutoResetEvent的情况下）或全部（ManualResetEvent的情况下）恢复执行。
     
     AutoResetEvent与ManualResetEvent的差别在于某个线程在WaitOne()被挂起后重新获得执行权时，是否自动reset这个
     事件(Event)，前者是自动reset的，后者不是。所以从这个角度上也可以解释上段提到的“在这个XXXResetEvent被set后，
     所有被挂起的线程中有一个（AutoResetEvent的情况下）或全部（ManualResetEvent的情况下）恢复执行”——因为前者
     一旦被某个线程获取后，立即自动reset这个事件(Event)，所以其他持有前者的线程之后WaitOne()时又被挂起；而后者
     被某个获取后，不会自动reset这个事件(Event)，所以后续持有后者的线程在WaitOne()时不会被挂起。
         */
    class MyAutoResetEvent
    {
        /*  
         * 构造方法的参数设置成false后，表示创建一个没有被set的AutoResetEvent 
         * 这就导致所有持有这个AutoResetEvent的线程都会在WaitOne()处挂起 
         * 此时如果挂起的线程数比较多，那么你看一下自己的内存使用量……。 
         * 如果将参数设置成true，表示创建一个被set的AutoResetEvent 
         * 持有这个AutoResetEvent的线程们会竞争这个Event 
         * 此时，在其他条件满足的情况下 
         * 至少会有一个线程得到执行 
         * 而不是因得不到Event而导致所有线程都得不到执行 
        */
        static AutoResetEvent myResetEvent = new AutoResetEvent(false);
        static int _Count = 0;
        static void Run()
        {
            Thread myThread = null;
            for (int i = 0; i < 100; i++)
            {
                myThread = new Thread(new ThreadStart(MyThreadProc));
                myThread.Name = "Thread" + i;
                myThread.Start();
            }
            myResetEvent.Set();
            Console.Read();
        }
        static void MyThreadProc()
        {
            myResetEvent.WaitOne();
            _Count++;
            Console.WriteLine("In thread:{0},label={1}.", Thread.CurrentThread.Name, _Count);

            myResetEvent.Set();
        }
    }
    class MyManualResetEvent
    {
        /*  
         * 构造方法的参数设置成false后，表示创建一个没有被set的ManualResetEvent 
         * 这就导致所有持有这个ManualResetEvent的线程都会在WaitOne()处挂起 
         * 此时如果挂起的线程数比较多，那么你看一下自己的内存使用量……。 
         * 如果将参数设置成true，表示创建一个被set的ManualResetEvent 
         * 持有这个ManualResetEvent的线程们在其他条件满足的情况下 
         * 会同时得到执行（注意，是同时得到执行！所以本例中的_Count的结果一般是不正确的^_^） 
         * 而不是因得不到Event而导致所有线程都得不到执行 
        */
        static ManualResetEvent myResetEvent = new ManualResetEvent(false);
        static int _Count = 0;
        static void Run()
        {
            Thread myThread = null;
            for (int i = 0; i < 1000; i++)
            {
                myThread = new Thread(new ThreadStart(MyThreadProc));
                myThread.Name = "Thread" + i;
                myThread.Start();
            }
            myResetEvent.Set();
            Console.Read();
        }
        static void MyThreadProc()
        {
            myResetEvent.WaitOne();
            _Count++;
            /* 
             * 在new ManualResetEvent(false);的情况下 
             * 下面的输出结果可能比较诡异：多个线程都输出label=1000！ 
             * 一种可能的原因是多个线程在各自执行到_Count++后，被挂起 
             * 随后打印的_Count值就不是本线程中刚刚修改过的_Count值了。 
             */
            Console.WriteLine("In thread:{0},_Count={1}.", Thread.CurrentThread.Name, _Count);
        }
    }
}
