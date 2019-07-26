using ms_amqp.Entities;
using ms_amqp.Infrastructure;
using ms_amqp.Infrastructure.Interfaces;
using ms_amqp.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ms_amqp.Services
{
    public class ReceiverService : IReceiverService
    {
        private IDomainRabbitMQ _domainRabbitMQ;
        public ReceiverService(IDomainRabbitMQ domainRabbitMQ)
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

        public void ReceiverStartup()
        {
            var isRunning = true;
            while (isRunning)
                isRunning = MenageMessage();
        }
        public bool MenageMessage()
        {
            try
            {
                var messageReceivedSerialized = _domainRabbitMQ.ReceiveMessage(queue);
                if (!string.IsNullOrEmpty(messageReceivedSerialized))
                {
                    var messageReceived = JsonConvert.DeserializeObject<Message>(messageReceivedSerialized);
                    Console.WriteLine($"ReceiverServiceName:{senderName} | ServiceSenderId:{messageReceived.senderId} | MessageTimestamp:{messageReceived.timeStamp} | Message:{messageReceived.message} | MessageGUID:{messageReceived.guid}");
                }
                Thread.Sleep(100);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
