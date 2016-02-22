using System.Collections.Generic;
using System.Text;
using MessageQueue;
using RabbitMQ.Client.Events;

namespace AccountOpenService
{
    public class AccountService
    {
        private readonly Listener _listener;
        private readonly Publisher _publisher;

        public AccountService()
        {
            _listener = new Listener("AccountOpenRequest", AccountOpenEventHandler);
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

        private void AccountOpenEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);

            var headers = new Dictionary<string, object> {{"ResponseStatus", "Success"}};
           
            _publisher.PublishEvent("AccountOpenResponse", message, ea.BasicProperties.CorrelationId, headers);
        }
    }
}
