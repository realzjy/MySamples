using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeEncodingFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Write("begin");

            string filename = ".\\1028phrases.all.bin";
            FileStream stream = new FileStream(filename, FileMode.Open);
            byte[] bytes= new byte[stream.Length];
            var r = stream.Read(bytes, 0, (int)stream.Length);

            //byte[] Bytes = Encoding.UTF8.GetBytes("你好!");

            //string str = Encoding.UTF7.GetString(bytes);
            string str = UnicodeEncoding.UTF32.GetString(bytes);
            Write(str);

            Write("end");
            Console.ReadLine();
        }

        private static void Write(string message) { Console.WriteLine(message); }
    }
}
