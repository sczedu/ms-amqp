using FizzWare.NBuilder;
using Moq;
using ms_amqp.Entities;
using ms_amqp.Infrastructure.Interfaces;
using ms_amqp.Interfaces;
using ms_amqp.Services;
using Newtonsoft.Json;
using System;
using Xunit;

namespace ms_amqp.Tests
{
    public class CoreServiceTest
    {
        private Mock<IDomainRabbitMQ> _domainRabbitMQ;
        private IReceiverService _receiverService;
        private ISenderService _senderService;

        public CoreServiceTest()
        {
            _domainRabbitMQ = new Mock<IDomainRabbitMQ>();
            _receiverService = new ReceiverService(_domainRabbitMQ.Object);
            _senderService = new SenderService(_domainRabbitMQ.Object);
        }

        [Fact]
        public void Test_Receiver_MenageMessage_OK()
        {
            var message = Builder<Message>.CreateNew().Build() as Message;

            var messageJson = JsonConvert.SerializeObject(message);

            _domainRabbitMQ.Setup(s => s.ReceiveMessage(It.IsAny<String>())).Returns(messageJson);

            var result = _receiverService.MenageMessage();

            Assert.True(result);
        }

        [Fact]
        public void Test_Receiver_MenageMessage_Exception()
        {
            _domainRabbitMQ.Setup(s => s.ReceiveMessage(It.IsAny<String>())).Throws(new Exception("Exception"));

            var result = _receiverService.MenageMessage();

            Assert.False(result);
        }

        [Fact]
        public void Test_Sender_MenageMessage_OK()
        {
            var message = Builder<Message>.CreateNew().Build() as Message;

            var messageJson = JsonConvert.SerializeObject(message);

            _domainRabbitMQ.Setup(s => s.SendMessage(It.IsAny<String>(), messageJson)).Returns(true);

            var result = _senderService.MenageMessage();

            Assert.True(result);
        }

        [Fact]
        public void Test_Sender_MenageMessage_Exception()
        {
            _domainRabbitMQ.Setup(s => s.ReceiveMessage(It.IsAny<String>())).Throws(new Exception("Exception"));

            var result = _senderService.MenageMessage();

            Assert.False(result);
        }

        [Fact]
        public void Test_Send_Stop_When_IsNotOK()
        {
            _domainRabbitMQ.Setup(s => s.SendMessage(It.IsAny<String>(), It.IsAny<String>())).Throws(new Exception("Exception"));

            _senderService.SenderStartup();

            Assert.True(true);
        }

        [Fact]
        public void Test_Receive_Stop_When_IsNotOK()
        {
            _domainRabbitMQ.Setup(s => s.ReceiveMessage(It.IsAny<String>())).Throws(new Exception("Exception"));

            _receiverService.ReceiverStartup();

            Assert.True(true);
        }

    }
}
