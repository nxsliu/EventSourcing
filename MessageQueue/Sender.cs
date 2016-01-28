﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace MessageQueue
{
    public class Sender
    {
        public void SendCommand(string queueName, string message, string correlationId, IDictionary<string, object> headers)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var basicProp = new BasicProperties();
                    basicProp.CorrelationId = correlationId;
                    basicProp.Headers = headers;
                    //basicProp.Headers = new Dictionary<string, object>();
                    //basicProp.Headers.Add(new KeyValuePair<string, object>("ApplicationType", "SuperSaver"));

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: basicProp,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}