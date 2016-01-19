﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "ApplicationSubmission",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var basicProp = new BasicProperties();
                    basicProp.CorrelationId = Guid.NewGuid().ToString();
                    basicProp.Headers = new Dictionary<string, object>();                                                            
                    basicProp.Headers.Add(new KeyValuePair<string, object>("ApplicationType", "SuperSaver"));

                    string message = File.ReadAllText(@"../../Application.json");
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "ApplicationSubmission",
                                         basicProperties: basicProp,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
