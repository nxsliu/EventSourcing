﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using RabbitMQ.Client.Events;

namespace Phase1
{
    public class Phase1Service
    {
        private readonly Listener _listener;
        private readonly Publisher _publisher;

        public Phase1Service()
        {
            _listener = new Listener("Phase1Request", Phase1EventHandler);
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

        private void Phase1EventHandler(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var headers = ea.BasicProperties.Headers;
            headers.Add("ResponseStatus", "Success");

            var message = Encoding.UTF8.GetString(body);

            _publisher.PublishEvent("Phase1Response", message, ea.BasicProperties.CorrelationId, headers);
        }
    }
}
