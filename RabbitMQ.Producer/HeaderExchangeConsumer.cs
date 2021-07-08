using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public static class HeaderExchangePublisher
    {
        public static void Publish(IModel channel)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };

            channel.ExchangeDeclare("demo-header-exchange", ExchangeType.Topic, arguments: ttl);

            var count = 0;
            while (true)
            {
                var message = new { Name = "Product", Message = $"Hello! Count: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> { { "account", "new" } };

                channel.BasicPublish("demo-header-exchange", string.Empty, properties, body);
                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
