using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 发布一个Http
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = @"http://localhost:50005/test/";

            HttpListener listener = new HttpListener();
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("listening...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                Console.WriteLine("{0} {1} HTTP/1.1", request.HttpMethod, request.RawUrl);
                Console.WriteLine("Accept: {0}", string.Join(",", request.AcceptTypes));
                //Console.WriteLine("Accept-Language: {0}", string.Join(",", request.UserLanguages));
                Console.WriteLine("User-Agent: {0}", request.UserAgent);
                Console.WriteLine("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                Console.WriteLine("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "close");
                Console.WriteLine("Host: {0}", request.UserHostName);
                Console.WriteLine("Pragma: {0}", request.Headers["Pragma"]);

                HttpListenerResponse response = context.Response;
                string responseString =
                    //@"<html><head><title>From HttpListener Server</title></head><body><h1>Hello, world.</h1></body></html>";
                    "test that this context is right message";
                response.ContentLength64 = System.Text.Encoding.UTF8.GetByteCount(responseString);
                response.ContentType = "text/html; charset=UTF-8";
                System.IO.Stream output = response.OutputStream;
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output);
                writer.Write(responseString);
                writer.Close();
                if (Console.KeyAvailable) { break; }
            }
            listener.Stop();

            Console.ReadLine();
        }
    }
}
