# rabbitmq_csharp
## A straightforward library do use rabbitmq with C# without several of it's complexities

This is not a complete RabbitMQ library, just because it's not. We have amazing options for this on the official RabbitMQ documentation:

https://www.rabbitmq.com/devtools.html#dotnet-dev

They are all amazing and they work. Why did I build this one though? 

RabbitMQ is a huge piece of software. RabbitMQ it's not a messaging system, like MSMQ or ZeroMQ. That's a big mistake an that's why most of developers struggle on trying to use it. RabbitMQ is so powerful that the 99% of the examples out there intend to use on it's pure Enterprise Service Bus (ESB) form. At some point, you will find yourself needing a simple mechanism to trade messages on a reliable way and ending up with a big ESB monster.

However, RabbitMQ CAN be used as messaging system. You can send and receive messages on the same fashion as the alternatives mentioned above (not a 100% the same, but that's ok). And that is not a bad thing, at all! It will help some developers to start using RabbitMQ as they figure other functionalities of this amazing AMQP system.

That's is the real goal of this library. Get your .NET Core application sending and receiving messages to and from RabbitMQ and then you can improve it as you go.

I hope you like it!

:)

## Other stuff

As I was making experiments, I've tried other libraries and some projects on this repository reflect that. To be concise, I've named the project with $"{purpose}_{library}". As you might see, at least on the first commit, I've made some experimets with the masstransit library. If you don't want to use it, don't worry. Just ignore it.