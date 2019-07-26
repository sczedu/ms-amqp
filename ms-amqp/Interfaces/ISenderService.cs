using System;
using System.Collections.Generic;
using System.Text;

namespace ms_amqp.Interfaces
{
    public interface ISenderService
    {
        void SenderStartup();
        bool MenageMessage();
    }
}
