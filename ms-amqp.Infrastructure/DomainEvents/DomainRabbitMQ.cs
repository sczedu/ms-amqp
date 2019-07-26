using ms_amqp.Infrastructure.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ms_amqp.Infrastructure
{
    public class DomainRabbitMQ : IDomainRabbitMQ
    {
        private string _rabbitmqHostname;
        private string rabbitmqHostname
        {
            get
            {
                if(String.IsNullOrWhiteSpace(_rabbitmqHostname))
                    _rabbitmqHostname = Environment.GetEnvironmentVariable("rabbitmqHostname");
                return _rabbitmqHostname;
            }
        }

        private string _rabbitmqUserName;
        private string rabbitmqUserName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_rabbitmqUserName))
                    _rabbitmqUserName = Environment.GetEnvironmentVariable("rabbitmqUserName");
                return _rabbitmqUserName;
            }
        }

        private string _rabbitmqPassword;
        private string rabbitmqPassword
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_rabbitmqPassword))
                    _rabbitmqPassword = Environment.GetEnvironmentVariable("rabbitmqPassword");
                return _rabbitmqPassword;
            }
        }

        public bool SendMessage(string queueName, string message)
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = rabbitmqHostname,
                    UserName = rabbitmqUserName,
                    Password = rabbitmqPassword,
                    RequestedHeartbeat = 60
                };
                using (var connection = connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: body);
                }
                return true;
            }
            catch(Exception ex)
            {
                //LOG DA EXCEPTION
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string ReceiveMessage(string queueName)
        {
            try
            {
                var messageReceived = "";
                var connectionFactory = new ConnectionFactory() { HostName = rabbitmqHostname, UserName = rabbitmqUserName, Password = rabbitmqPassword };
                using (var connection = connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        messageReceived = Encoding.UTF8.GetString(body);
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                }

                return messageReceived;
            }
            catch(Exception ex)
            {
                //LOG DA EXCEPTION
                return null;
            }
        }
    }
}
