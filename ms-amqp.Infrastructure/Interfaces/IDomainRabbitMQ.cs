using System;
using System.Collections.Generic;
using System.Text;

namespace ms_amqp.Infrastructure.Interfaces
{
    public interface IDomainRabbitMQ
    {
        bool SendMessage(string queueName, string message);
        string ReceiveMessage(string queueName);
    }
}
