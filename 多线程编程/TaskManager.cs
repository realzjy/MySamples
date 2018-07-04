using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程编程
{
    class TaskManager
    {
        public async void Run()
        {
            Console.WriteLine("main sync begin: " + "...");

            int r = await OtherRun();

            //TaskAwaiter<int> ta = t.GetAwaiter();
            //int r = ta.GetResult();
            Console.WriteLine("result: " + r);

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("main sync finish: " + "...");
        }
        private async Task<int> OtherRun()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("async: " + i + "...");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            return Task.CurrentId == null ? 0 : Task.CurrentId.Value;
        }
    }
}
