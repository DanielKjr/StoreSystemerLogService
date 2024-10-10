using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Text;

namespace LogService
{
    public class MessageBroker
    {
        string url = "rabbitmq";

        private ConnectionFactory factory;

        private IModel channel;

        private IConnection connection;

        public MessageBroker()
        {
            factory = new ConnectionFactory() { HostName = url, Port = 5672 };
            connection = factory.CreateConnection();
            CreateChannel("Log", ExchangeType.Topic, "Transaction");
        }

        public void CreateChannel(string exchangeName, string exchangeType, string routingKey)
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);

            var queueName = channel.QueueDeclare(autoDelete: false).QueueName;

            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            this.channel = channel;
        }
    }
}
