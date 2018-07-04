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
    /// 长连接的Socket示例
    /// </summary>
    public class LongServerExample
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

        public static void StartListening()
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

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(GetIPEndPoint());
            listener.Listen(100);
            Console.WriteLine("Waiting for a connection..." + GetTime());
            listener.BeginAccept(
               (ar) =>
               {
                   Socket lisAcc = (Socket)ar.AsyncState;
                   workSocket = lisAcc.EndAccept(ar);
                   workSocket.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), workSocket);
               },
               listener);
        }
        private static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            int bytesRead = workSocket.EndReceive(ar);
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
                long serialID = System.BitConverter.ToInt64(blist.ToArray(), 0);
                long identityID = System.BitConverter.ToInt64(blist.ToArray(), 8);
                //头标识(Ox5b) 消息头 消息体 尾标识(0x5d)
                byte messageID = 0xA1;
                byte identity = 0x00;
                if (identityID == 22 || identityID == 33)
                { identity = 0x01; }

                List<byte> rlist = new List<byte>();
                rlist.Add(messageID);
                rlist.Add(identity);
                byte[] serialBs = System.BitConverter.GetBytes(serialID);
                rlist.AddRange(serialBs.ToList());

                List<byte> replaceBList = GetReplaceByteList(rlist);

                Send(workSocket, replaceBList.ToArray());

                if (identityID == 22 || identityID == 33)
                {
                    List<byte> contents = new List<byte>();
                    contents.Add(0xB1);
                    string message = "server send message to client: hello i am server";
                    byte[] bs = Encoding.GetEncoding("GBK").GetBytes(message);
                    bs.Reverse();
                    contents.AddRange(bs);
                    //double tms = Convert.ToInt64(System.DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds);
                    //byte[] tmsB = System.BitConverter.GetBytes(tms);
                    //rlist.AddRange(serialBs.ToList());
                    //contents.AddRange(tmsB);

                    Send(workSocket, contents.ToArray());
                }
                workSocket.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), workSocket);
            }
        }

        private static List<byte> GetReplaceByteList(List<byte> rlist)
        {
            List<byte> replaceBList = new List<byte>();
            replaceBList.Add(0x5b);
            foreach (byte item in rlist)
            {
                switch (item)
                {
                    case 0x5b:
                        replaceBList.Add(0x5e);
                        replaceBList.Add(0x01);
                        break;
                    case 0x5d:
                        replaceBList.Add(0x5e);
                        replaceBList.Add(0x02);
                        break;
                    case 0x5e:
                        replaceBList.Add(0x5e);
                        replaceBList.Add(0x00);
                        break;
                    default:
                        replaceBList.Add(item);
                        break;
                }
            }
            replaceBList.Add(0x5d);
            return replaceBList;
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
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    },
                socket);
        }
        private static void Send(Socket socket, byte[] bytes)
        {
            socket.BeginSend(
                bytes,
                0,
                bytes.Length,
                0,
                (ar) =>
                {
                    try
                    {
                        Socket socketSend = (Socket)ar.AsyncState;
                        int bytesSent = socketSend.EndSend(ar);
                        Console.WriteLine("server send back {0} bytes to client.{1}", bytes.Length, GetTime());
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