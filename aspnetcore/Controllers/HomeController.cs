using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using domain;
using messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMessagePublisher<GenericMessage> _messagePublisher;

        public HomeController(IMessagePublisher<GenericMessage> messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [Route("publish")]
        public ActionResult PostMessage([FromQuery]Guid messageId)
        {
            this._messagePublisher.SendMessage(new GenericMessage() { Identifier = messageId });
            return Ok();
        }
    }
}