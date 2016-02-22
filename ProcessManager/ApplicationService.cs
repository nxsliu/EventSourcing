using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore;
using MessageQueue;
using Newtonsoft.Json;
using ProcessManager.Events;
using ProcessManager.Products;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProcessManager
{
    public class ApplicationService
    {
        private readonly IList<Listener> _listeners = new List<Listener>();

        public ApplicationService()
        {
            _listeners.Add(new Listener("ApplicationSubmission", EventHandlers.StartApplicationEventHandler));
            _listeners.Add(new Listener("InternalCheckResponse", EventHandlers.InternalCheckResponseEventHandler));
            _listeners.Add(new Listener("CreditCheckResponse", EventHandlers.CreditCheckResponseEventHandler));
            _listeners.Add(new Listener("AccountOpenResponse", EventHandlers.AccountOpenResponseEventHandler));
        }

        public void Start()
        {
            foreach (var listener in _listeners)
            {
                listener.Start();
            }           
        }

        public void Stop()
        {
            foreach (var listener in _listeners)
            {
                listener.Stop();
            }
        }        
    }
}
