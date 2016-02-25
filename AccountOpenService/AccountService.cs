using System;
using System.Collections.Generic;
using System.Text;
using MessageQueue;
using Newtonsoft.Json;
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

            var application = JsonConvert.DeserializeObject<Application>(message);

            string responseMessage;

            if (application.InternalCheck)
            {
                responseMessage =
                    JsonConvert.SerializeObject(
                        new
                        {
                            ApplicationId = application.Id,
                            AccountOpened = true,
                            AccountNumber = Guid.NewGuid(),
                            BranchNumber = new Random().Next(0, 1000000).ToString("D6")
                        });
            }
            else
            {
                responseMessage = JsonConvert.SerializeObject(new { ApplicationId = application.Id, AccountOpened = false });
            }

            _publisher.PublishEvent("AccountOpenResponse", responseMessage, ea.BasicProperties.CorrelationId, ea.BasicProperties.Headers);
        }
    }
}
