using System.Collections.Generic;
using System.Text;
using MessageQueue;
using Newtonsoft.Json;
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

            var checker = JsonConvert.DeserializeObject<Checker>(message);

            string responseMessage;

            if (checker.AnnualIncome > 40000)
            {
                responseMessage = JsonConvert.SerializeObject(new { ApplicationId = checker.Id, CreditCheck = true });
            }
            else
            {
                responseMessage = JsonConvert.SerializeObject(new { ApplicationId = checker.Id, CreditCheck = false });
            }

            _publisher.PublishEvent("CreditCheckResponse", responseMessage, ea.BasicProperties.CorrelationId, ea.BasicProperties.Headers);
        }
    }
}
