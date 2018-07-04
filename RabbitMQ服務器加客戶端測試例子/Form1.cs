using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQ服務器加客戶端測試例子
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        RabbitMQService _mq = null;
        RabbitMQDeploy _depoy = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            _depoy = new RabbitMQDeploy();

            _depoy.HostName = "localhost";
            _depoy.Port = 5672;
            _depoy.VirtualHost = "/zjytest";
            _depoy.UserName = "test";
            _depoy.Password = "test";
            _depoy.AutoClose = false;
            _depoy.RequestedConnectionTimeout = 10000;
            _depoy.RequestedHeartbeat = 10;

            _depoy.QueueName = "exchange.zjy.001";
            _depoy.ClientServiceName = "localhost";
            _depoy.ExchangeType = "direct";
            _depoy.ExchangeDurable = false;
            _depoy.ExchangeAutoDelete = false;
            _depoy.ExchangeArguments = null;

            _depoy.QueueName = "queue.zjy.001";
            _depoy.QueueDurable = true;
            _depoy.QueueExclusive = false;
            _depoy.QueueAutoDelete = false;
            _depoy.QueueNoAck = false;
            _depoy.QueueArguments = null;

            _depoy.ExchangeName = _depoy.QueueName;
            _depoy.QueueName = _depoy.QueueName;
            _depoy.RoutingKey = "";

            if (_mq == null) { _mq = new RabbitMQService(_depoy); }
        }

        private string GetString(int lenght) { return new string('X', lenght); }

        private void bSend_Click(object sender, EventArgs e)
        {
            byte[] s10 = StringCodeChange.SerializeObject(GetString(10));
            _mq.Publish(s10);
            _mq.Disconnect();
        }

        private void bReceive_Click(object sender, EventArgs e)
        {
            if (_mq == null) { _mq = new RabbitMQService(_depoy); }
            _mq.Connect();
            _mq.StartConsume(_depoy);
            _mq.Received +=
                (object sR, RabbitMQ.Client.Events.BasicDeliverEventArgs eR) =>
                {
                    string rStr = StringCodeChange.DeserializeObject<string>(eR.Body);
                    this.Invoke(new SetValue((o) => { textBox1.Text += o + "\r\n"; }), rStr);

                    //textBox1.Text += rStr + "\r\n";
                    _mq.StopConsume();
                };
        }
        public delegate void SetValue(string txt);

        private void bOpenQ_Click(object sender, EventArgs e)
        {
            _mq.Connect();
        }

        private void bCloseQ_Click(object sender, EventArgs e)
        {
            _mq.Disconnect();
        }
    }
}
