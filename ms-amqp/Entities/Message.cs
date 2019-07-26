using System;
using System.Collections.Generic;
using System.Text;

namespace ms_amqp.Entities
{
    public class Message
    {
        public string message { get; set; }
        public string senderId { get; set; }
        public DateTime timeStamp { get; set; }
        public Guid guid { get; set; }
    }
}
