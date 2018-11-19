using domain;
using messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace generichost
{
    public class GenericProcessor : IProcessor<GenericMessage>
    {
        private readonly ILogger _logger;

        public GenericProcessor(ILogger<GenericProcessor> logger)
        {
            _logger = logger;
        }

        public async Task OnError(GenericMessage message, Exception ex)
        {
            _logger.LogError(ex, "Oops... something is wrong here");
        }

        public async Task Process(GenericMessage message)
        {
            Console.WriteLine($"Hey, I've got a message with the Identifier {message.Identifier}");
        }
    }
}
