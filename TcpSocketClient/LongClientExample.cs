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
    /// 长连接的Socket示例
    /// </summary>
    public class LongClientExample
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
        
        private static String response = String.Empty;
        public static void StartClient()
        {
            Thread t = new Thread(
                () =>
                {
                    while (true)
                    {
                        Console.WriteLine(string.Format("socket {0}.", workSocket != null && workSocket.Connected ? "open" : "closed"));
                        Thread.Sleep(10000);
                    }
                }
                );
            //t.Start();

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
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    },
                    client);
            //Send(client, "This is a test");
            client.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), client);
            Console.ReadLine();
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = workSocket.EndReceive(ar);
            string result = string.Empty;
            if (bytesRead > 0)
            {
                List<byte> blist = new List<byte>();
                if (buffer.FirstOrDefault() == 0x5b || buffer[bytesRead - 1] == 0x5d)
                {
                    for (int i = 1; i < bytesRead - 1; i++)
                    {
                        if (buffer[i] == 0x5e)
                        {
                            switch (buffer[i + 1])
                            {
                                case 0x01:
                                    blist.Add(0x5b);
                                    break;
                                case 0x02:
                                    blist.Add(0x5d);
                                    break;
                                case 0x00:
                                    blist.Add(0x5e);
                                    break;
                            }
                        }
                        else
                        { blist.Add(buffer[i]); }
                    }

                }

                if (blist[0] == 0xA1)
                {
                    byte identity = blist[1];
                    long serialID = System.BitConverter.ToInt64(blist.ToArray(), 2);
                    Console.WriteLine(string.Format("receive 0xA1: indentity {0}, serial {1}.", identity, serialID));
                }
                if (blist[0] == 0xB1)
                {
                    blist.Reverse();
                    blist.RemoveAt(blist.Count - 1);
                    response = Encoding.GetEncoding("GBK").GetString(blist.ToArray());
                }

                //response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Response received : {0}", response);
                workSocket.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), workSocket);
            }
        }
        public static void Send(Socket socket, String data)
        {
            data += "<EOF>";
            if (socket == null)
            { socket = workSocket; }
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            socket.BeginSend(
                byteData, 
                0, 
                byteData.Length, 
                0, 
                (ar)=>
                {
                    Socket client = (Socket)ar.AsyncState;
                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("cilent sent {0} to server.", data);
                },
                socket);
        }
        public static void Send(Socket socket, byte[] bytes)
        {
            if (socket == null)
            { socket = workSocket; }
            socket.BeginSend(
                bytes,
                0,
                bytes.Length,
                0,
                (ar) =>
                {
                    Socket client = (Socket)ar.AsyncState;
                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("cilent sent {0} bytes to server.", bytes.Length);
                },
                socket);
        }
    }
}