using MassTransit;
using System;

namespace generichost
{
    public class ConsumerObserver : IObserver<ConsumeContext<MessageModel>>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Oops... something went wrong");
        }

        public void OnNext(ConsumeContext<MessageModel> value)
        {
            Console.WriteLine("Customer address was updated: {0}", value.Message.Identifier);
        }
    }
}
