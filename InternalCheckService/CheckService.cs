using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using RabbitMQ.Client.Events;

namespace InternalCheckService
{
    public class CheckService
    {
        private readonly Listener _listener;
        private readonly Publisher _publisher;

        public CheckService()
        {
            _listener = new Listener("InternalCheckRequest", InternalCheckEventHandler);
            _publisher = new Publisher();
        }

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void InternalCheckEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);

            var headers = new Dictionary<string, object> {{"ResponseStatus", "Success"}};

            _publisher.PublishEvent("InternalCheckResponse", message, ea.BasicProperties.CorrelationId, headers);
        }
    }
}
