using ms_amqp.Infrastructure;
using ms_amqp.Entities;
using System.Threading;
using System;
using Newtonsoft.Json;
using ms_amqp.Interfaces;
using ms_amqp.Infrastructure.Interfaces;

namespace ms_amqp.Services
{
    public class SenderService : ISenderService
    {
        private IDomainRabbitMQ _domainRabbitMQ;
        public SenderService(IDomainRabbitMQ domainRabbitMQ)
        {
            _domainRabbitMQ = domainRabbitMQ;
        }

        private string _queue;
        private string queue
        {
            get
            {
                if (string.IsNullOrEmpty(_queue))
                    _queue = Environment.GetEnvironmentVariable("queueName");
                return _queue;
            }
        }

        private string _senderName;
        private string senderName
        {
            get
            {
                if (string.IsNullOrEmpty(_senderName))
                    _senderName = Environment.GetEnvironmentVariable("serviceName");
                return _senderName;
            }
        }

        public void SenderStartup()
        {
            var isRunning = true;
            while (isRunning)
                isRunning = MenageMessage();
        }

        public bool MenageMessage()
        {
            try
            {
                var message = new Message()
                {
                    guid = Guid.NewGuid(),
                    message = "Hello World",
                    senderId = senderName,
                    timeStamp = DateTime.Now
                };
                var messageJson = JsonConvert.SerializeObject(message);
                _domainRabbitMQ.SendMessage(queue, messageJson);
                Thread.Sleep(5000);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
