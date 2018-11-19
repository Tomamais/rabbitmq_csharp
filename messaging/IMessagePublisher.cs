using System;
using System.Collections.Generic;
using System.Text;

namespace messaging 
{
    public interface IMessagePublisher<T>
    {
        void SendMessage(T message);
    }
}
