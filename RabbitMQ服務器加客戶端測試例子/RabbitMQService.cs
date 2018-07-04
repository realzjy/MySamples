using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ服務器加客戶端測試例子
{
    class RabbitMQService
    {
        private IConnection _connection = null;
        private IModel _channel = null;
        protected readonly object _pipelining = new object();

        RabbitMQDeploy _deploy = null;
        
        public RabbitMQService(RabbitMQDeploy deploy) { _deploy = deploy; }

        public void Connect()
        {
            var connectionFactory = BuildConnectionFactory(_deploy);
            _connection = connectionFactory.CreateConnection();
            _connection.AutoClose = false;
            _connection.ConnectionShutdown += OnConnectionShutdown;
            BindChannel(_deploy);
        }

        private ConnectionFactory BuildConnectionFactory(RabbitMQDeploy deploy)
        {
            var factory = new ConnectionFactory()
            {
                Protocol = Protocols.DefaultProtocol,
                HostName = deploy.HostName,
                Port = deploy.Port,
                VirtualHost = deploy.VirtualHost,
                UserName = deploy.UserName,
                Password = deploy.Password,
                RequestedConnectionTimeout = deploy.RequestedConnectionTimeout,
                RequestedHeartbeat = deploy.RequestedHeartbeat,
            };
            factory.ClientProperties.Add("Application Name", deploy.ClientServiceName);
            factory.ClientProperties.Add("Application Connected Time (UTC)", DateTime.UtcNow.ToString("o"));
            return factory;
        }


        private void BindChannel(RabbitMQDeploy deploy)
        {
            _channel = _connection.CreateModel();
            _channel.ModelShutdown += OnChannelShutdown;
            _channel.ExchangeDeclare(deploy.ExchangeName, deploy.ExchangeType, deploy.ExchangeDurable, deploy.ExchangeAutoDelete, deploy.ExchangeArguments);
            var queueStatus = _channel.QueueDeclare(deploy.QueueName, deploy.QueueDurable, deploy.QueueExclusive, deploy.QueueAutoDelete, deploy.QueueArguments);
            _channel.QueueBind(deploy.QueueName, deploy.ExchangeName, deploy.RoutingKey);
            _channel.BasicQos(0, 1, false);
        }

        public void Disconnect()
        {
            if (_channel != null)
            {
                _channel.ModelShutdown -= OnChannelShutdown;
                _channel.Close();
            }
            if (_connection != null)
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.Close();
            }
        }

        protected virtual void OnChannelShutdown(object sender, ShutdownEventArgs e) { Disconnect(); }

        protected virtual void OnConnectionShutdown(object sender, ShutdownEventArgs e) { Disconnect(); }

        public void Publish(byte[] message)
        {
            _channel.BasicPublish(_deploy.ExchangeName, _deploy.RoutingKey, false, null, message);
        }


        EventingBasicConsumer _consumer = null;
        private Action _recoverConsume = null;
        private string _consumerTag;

        public void StartConsume(RabbitMQDeploy deploy)
        {
            _consumer = new EventingBasicConsumer(_channel);
            _consumerTag = _channel.BasicConsume(deploy.QueueName, deploy.QueueNoAck, _consumer);
            _consumer.Received += OnReceived;
            _consumer.Shutdown += OnShutdown;
            _recoverConsume = () => { _consumerTag = _channel.BasicConsume(_deploy.QueueName, _deploy.QueueNoAck, _consumerTag, _consumer); };
        }

        public void StopConsume()
        {
            if (_consumer != null)
            {
                _consumer.Received -= OnReceived;
                _consumer.Shutdown -= OnShutdown;

                if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
                {
                    _channel.BasicCancel(_consumerTag);
                }
            }
        }

        private void OnShutdown(object sender, ShutdownEventArgs e) { StopConsume(); }

        private void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            Received(sender, e);
            _channel.BasicAck(e.DeliveryTag, true);
        }

        public event EventHandler<BasicDeliverEventArgs> Received;
    }
}
