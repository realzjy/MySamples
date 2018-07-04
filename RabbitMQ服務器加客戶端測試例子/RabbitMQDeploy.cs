using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ服務器加客戶端測試例子
{
    class RabbitMQDeploy
    {
        #region Rabbit 服務主機配置
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool AutoClose { get; set; }
        public int RequestedConnectionTimeout { get; set; }
        public ushort RequestedHeartbeat { get; set; }
        #endregion

        #region Exchange 配置
        public string ClientServiceName { get; set; }
        public string ExchangeType { get; set; }
        public bool ExchangeDurable { get; set; }
        public bool ExchangeAutoDelete { get; set; }
        public Dictionary<string, object> ExchangeArguments { get; set; }
        #endregion

        #region Queue 配置
        public bool QueueDurable { get; set; }
        public bool QueueExclusive { get; set; }
        public bool QueueAutoDelete { get; set; }
        public bool QueueNoAck { get; set; }
        public Dictionary<string, object> QueueArguments { get; set; }
        #endregion

        #region Exchange 和 Queue 配置
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        #endregion
    }
}
