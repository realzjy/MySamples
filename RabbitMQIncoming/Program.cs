using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQIncoming
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, object> ExchangeArguments = null;
            Dictionary<string, object> QueueArguments = null;

            var factory = new ConnectionFactory()
            {
                Protocol = Protocols.DefaultProtocol,
                HostName = "localhost",
                Port = 5672,
                VirtualHost = @"/dev",
                UserName = "guest",
                Password = "guest",
                AutomaticRecoveryEnabled = true,
                //RequestedConnectionTimeout = UInt16.MaxValue,
                //RequestedHeartbeat = UInt16.MaxValue,
            };
            var connect = factory.CreateConnection();



            var AutoClose = false;
            var ExchangeName = "rmq.exchange.001";
            var QueueName = "rmq.queue..001";
            var RoutingKey = "";
            var ExchangeType = "direct";
            var ExchangeDurable = false;
            var ExchangeAutoDelete = false;
            var QueueDurable = true;
            var QueueExclusive = false;
            var QueueAutoDelete = false;
            connect.AutoClose = AutoClose;
            var channel = connect.CreateModel();
            if (!string.IsNullOrEmpty(ExchangeName))
            {
                channel.ExchangeDeclare(ExchangeName, ExchangeType, ExchangeDurable, ExchangeAutoDelete, ExchangeArguments);
            }

            if (!string.IsNullOrEmpty(QueueName))
            {
                var queueStatus = channel.QueueDeclare(QueueName, QueueDurable, QueueExclusive, QueueAutoDelete, QueueArguments);
                channel.QueueBind(QueueName, ExchangeName, RoutingKey);
                channel.BasicQos(0, 1, false);
            }

            List<Task> ts = new List<Task>();
            for (int i2 = 0; i2 < 10000; i2++)
            {
                Task t = new Task(
                () =>
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        string str = string.Empty;
                        for (int l = 0; l < 100; l++) { str += "X"; }
                        byte[] message = ProtocolBuffersConvert.SerializeObject<string>(str);
                        channel.BasicPublish(ExchangeName, RoutingKey, false, null, message);
                    }
                });
                Console.WriteLine("push queue " + t.Id);
                ts.Add(t);
            }
            foreach (var i in ts) { i.Start(); }
            Task.WaitAll(ts.ToArray());

            Console.WriteLine("queue insert seccuss");

            Console.ReadLine();
        }
    }
}
