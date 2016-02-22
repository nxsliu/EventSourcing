using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using RabbitMQ.Client.Events;

namespace Phase2
{
    public class Phase2Service
    {
        private readonly Listener _listener;
        private readonly Publisher _publisher;

        public Phase2Service()
        {
            _listener = new Listener("Phase2Request", Phase2EventHandler);
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

        private void Phase2EventHandler(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var headers = ea.BasicProperties.Headers;
            headers.Add("ResponseStatus", "Success");

            var message = Encoding.UTF8.GetString(body);

            _publisher.PublishEvent("Phase2Response", message, ea.BasicProperties.CorrelationId, headers);
        }
    }
}
