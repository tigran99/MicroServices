using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public static class FanoutExchangePublisher
    {
        public class Order
        {
            public string Name { get; set; }
            public string Message { get; set; }
        }
        public static void Publish(IModel channel)
        {
            //var ttl = new Dictionary<string, object>
            //{
            //    {"x-message-ttl", 30000 }
            //};

            channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout/*, arguments: ttl*/);

            var count = 0;
            while (true)
            {
                var message = new Order { Name = "Product", Message = $"Hello! Count: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("demo-fanout-exchange", string.Empty, null, body);
                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
