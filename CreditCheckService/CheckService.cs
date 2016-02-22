using System.Collections.Generic;
using System.Text;
using MessageQueue;
using RabbitMQ.Client.Events;

namespace CreditCheckService
{
    public class CheckService
    {
        private readonly Listener _listener;
        private readonly Publisher _publisher;

        public CheckService()
        {
            _listener = new Listener("CreditCheckRequest", CreditCheckEventHandler);
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

        private void CreditCheckEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);

            var headers = new Dictionary<string, object> { { "ResponseStatus", "Success" } };            

            _publisher.PublishEvent("CreditCheckResponse", message, ea.BasicProperties.CorrelationId, headers);
        }
    }
}
