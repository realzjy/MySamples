using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpSocketServer
{
    class Program
    {
        /// <summary>
        /// server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //ServerExample.StartListening();

            LongServerExample.StartListening();

            Console.ReadLine();
        }
    }
}
