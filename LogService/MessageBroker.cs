using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.IO;
using System.Text;

namespace LogService
{
    public class MessageBroker
    {
        string url = "rabbitmq";

        private ConnectionFactory factory;

        private IModel channel;

        private IConnection connection;

        private string docPath;

        public MessageBroker()
        {
            factory = new ConnectionFactory() { HostName = url, Port = 5672 };
            connection = factory.CreateConnection();
            CreateChannel("Log", ExchangeType.Topic, "Transaction");
            docPath = Directory.GetCurrentDirectory();
        }

        public void CreateChannel(string exchangeName, string exchangeType, string routingKey)
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);

            var queueName = channel.QueueDeclare(autoDelete: false).QueueName;

            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Write the string array to a file named "TransactionLog.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "TransactionLog.txt"), true))
                {
                    outputFile.WriteLine(message);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            this.channel = channel;
        }
    }
}
