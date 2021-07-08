using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public static class FanoutExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout);
             var queue =  channel.QueueDeclare(
    durable: false,
    exclusive: false,
    autoDelete: true,
    arguments: null);

            channel.QueueBind(queue.QueueName, "demo-fanout-exchange", string.Empty, null);
            //channel.BasicQos(0, 10, false);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume(queue.QueueName, true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }
    }
}
