using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpSocketClient
{
    /// <summary>
    /// 传输完关闭的Socket示例
    /// </summary>
    public class ClientExample
    {
        public static Socket workSocket = null;
        public const int BufferSize = 1024;
        public static byte[] buffer = new byte[BufferSize];
        private const int Port = 55555;
        private static IPEndPoint GetIPEndPoint()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            var ip = ips.Where(o => o.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault().ToString();
            return new IPEndPoint(IPAddress.Parse(ip), Port);
        }
        private static string GetTime() { return " " + System.DateTime.Now.ToString("hh: mm: ss" + "." + System.DateTime.Now.Millisecond); }
        
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static String response = String.Empty;
        public static void StartClient()
        {
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(
                    GetIPEndPoint(),
                    (ar) => 
                        {
                            try
                            {
                                workSocket = (Socket)ar.AsyncState;
                                workSocket.EndConnect(ar);
                                Console.WriteLine("Socket connected to {0}", workSocket.RemoteEndPoint.ToString());
                                connectDone.Set();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        },
                        client);
                connectDone.WaitOne();
                Send(client, "This is a test<EOF>");
                sendDone.WaitOne();
                Receive(client);
                receiveDone.WaitOne();
                Console.WriteLine("Response received : {0}", response);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void Receive(Socket client)
        {
            client.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), client);
        }
        public static void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = workSocket.EndReceive(ar);
            string result = string.Empty;
            if (bytesRead > 0)
            {
                response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                workSocket.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), workSocket);
            }
            else
            {
                receiveDone.Set();
            }
        }
        public static void Send(Socket socket, String data)
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
                        Socket client = (Socket)ar.AsyncState;
                        int bytesSent = client.EndSend(ar);
                        Console.WriteLine("cilent sent {0} to server.", data);
                        sendDone.Set();
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