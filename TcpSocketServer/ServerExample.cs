using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpSocketServer
{
    /// <summary>
    /// 传输完关闭的Socket示例
    /// </summary>
    public class ServerExample
    {
        private static Socket workSocket = null;
        private const int BufferSize = 1024;
        private static byte[] buffer = new byte[BufferSize];
        private const int Port = 55555;
        private static IPEndPoint GetIPEndPoint()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            var ip = ips.Where(o => o.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault().ToString();
            return new IPEndPoint(IPAddress.Parse(ip), Port);
        }
        private static string GetTime() { return " " + System.DateTime.Now.ToString("hh: mm: ss" + "." + System.DateTime.Now.Millisecond); }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(GetIPEndPoint());
            listener.Listen(100);
            while (true)
            {
                allDone.Reset();
                Console.WriteLine("Waiting for a connection..." + GetTime());
                listener.BeginAccept(
                   (ar) =>
                   {
                       allDone.Set();
                       Socket lisAcc = (Socket)ar.AsyncState;
                       workSocket = lisAcc.EndAccept(ar);
                       workSocket.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), workSocket);
                   },
                   listener);
                allDone.WaitOne();
            }
            //Console.WriteLine("\nPress ENTER to continue...");
            //Console.Read();
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            int bytesRead = workSocket.EndReceive(ar);
            if (bytesRead > 0)
            {  
                content = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (content.IndexOf("<EOF>") > -1)
                {
                    Console.WriteLine("server read {0} from client.{1}", content, GetTime());
                    content = "server read :" + content;
                    Send(workSocket, content);
                }
                else
                {
                    workSocket.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), workSocket);
                }
            }
        }
        private static void Send(Socket socket, String data)
        {   
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            socket.BeginSend(
                byteData, 
                0, 
                byteData.Length, 
                0, 
                (ar)=>
                    {
                        try
                        {
                            Socket socketSend = (Socket)ar.AsyncState;
                            int bytesSent = socketSend.EndSend(ar);
                            Console.WriteLine("server send back {0} to client.{1}", data, GetTime());
                            socketSend.Shutdown(SocketShutdown.Both);
                            socketSend.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    },
                socket);
        }
    }
}