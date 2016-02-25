using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using Newtonsoft.Json;
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

            var checker = JsonConvert.DeserializeObject<Checker>(message);

            string responseMessage;

            if (checker.Email.ToLower().StartsWith(checker.Name.Substring(0, 1).ToLower()))
            {
                responseMessage = JsonConvert.SerializeObject(new {ApplicationId = checker.Id, InternalCheck = true});
            }
            else
            {
                responseMessage = JsonConvert.SerializeObject(new {ApplicationId = checker.Id, InternalCheck = false});
            }

            _publisher.PublishEvent("InternalCheckResponse", responseMessage, ea.BasicProperties.CorrelationId, ea.BasicProperties.Headers);
        }
    }
}
