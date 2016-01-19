using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topshelf.Builders;

namespace ProcessManager
{
    public class QueueListener
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly string _queueName;
        private readonly EventHandler<BasicDeliverEventArgs> _eventHandler;

        public QueueListener(string queueName, EventHandler<BasicDeliverEventArgs> eventHandler)
        {
            _queueName = queueName;
            _eventHandler = eventHandler;
        }

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += _eventHandler;
            _channel.BasicConsume(queue: _queueName,
                                 noAck: true,
                                 consumer: consumer);
        }

        public void Stop()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
