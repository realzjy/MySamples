using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpSocketClient
{
    class Program
    {
        static long serialID = 0;

        /// <summary>
        /// client
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //ClientExample.StartClient();

            LongClientExample.StartClient();

            while (true)
            {
                string str = Console.ReadLine();
                if (str == "send")
                {
                    //头标识(Ox5b) 消息头 消息体 尾标识(0x5d)
                    //0x5b <——> 0x5e后紧跟一个0x01；
                    //0x5d <——> 0x5e后紧跟一个0x02；
                    //0x5e <——> 0x5e后紧跟一个0x00

                    byte messageID = 0x01;
                    serialID = 0;
                    long identityID = 22;//33

                    List<byte> blist = new List<byte>();
                    blist.Add(messageID);
                    byte[] serialBs = System.BitConverter.GetBytes(serialID);
                    blist.AddRange(serialBs.ToList());
                    byte[] identityBs = System.BitConverter.GetBytes(identityID);
                    blist.AddRange(identityBs.ToList());

                    List<byte> replaceBList = new List<byte>();
                    replaceBList.Add(0x5b);
                    foreach (byte item in blist)
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


                    //byte messageID = 0x11;
                    //long contentLenghtID = 10;
                    //long timeID = 12345678;

                    try { LongClientExample.Send(LongClientExample.workSocket, replaceBList.ToArray()); }
                    catch (Exception ee) { Console.WriteLine(ee.Message); }
                    serialID++;
                }
            }
        }
    }
}
