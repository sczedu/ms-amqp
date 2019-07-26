using ms_amqp.Entities.Enum;
using ms_amqp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using ms_amqp.Services;
using ms_amqp.Infrastructure;
using ms_amqp.Infrastructure.Interfaces;

namespace ms_amqp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
           .AddScoped<ISenderService, SenderService>()
           .AddScoped<IReceiverService, ReceiverService>()
           .AddScoped<IDomainRabbitMQ, DomainRabbitMQ>()
           .BuildServiceProvider();

            var applicationType = Environment.GetEnvironmentVariable("applicationType");

            switch (applicationType.ToLower())
            {
                case ServiceEnum.Sender:
                    var senderService = serviceProvider.GetService<ISenderService>();
                    senderService.SenderStartup();
                    break;
                case ServiceEnum.Receiver:
                    var receiverService = serviceProvider.GetService<IReceiverService>();
                    receiverService.ReceiverStartup();
                    break;
            }

        }
    }
}
