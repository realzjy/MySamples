using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("demo begin..");
            AsyncMethod();
            Thread.Sleep(1000);
            Console.WriteLine("demo end..");
            Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///     带有async关键的方法中需要等待有异步线程的函数，
        ///     所以在async关键字内的函数需要使用await关键字来执行需要等待的异步方法。
        /// </remarks>
        static async void AsyncMethod()
        {
            Console.WriteLine("sync run");
            var result = await MyMethod();
            Console.WriteLine("async run over");
        }

        static async Task<int> MyMethod()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("async run " + i.ToString() + " secound..");
                await Task.Delay(1000); //模拟耗时操作
            }
            return 0;
        }
    }
}
