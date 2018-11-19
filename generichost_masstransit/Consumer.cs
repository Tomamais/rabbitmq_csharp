using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace generichost
{
    public class Consumer : IConsumer<MessageModel>
    {
        public async Task Consume(ConsumeContext<MessageModel> context)
        {
            await Console.Out.WriteLineAsync($"Consuming message: {context.Message.Identifier}");
        }
    }
}
