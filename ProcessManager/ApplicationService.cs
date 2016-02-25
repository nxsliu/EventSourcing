using System;
using System.Collections.Generic;
using System.Linq;
using MessageQueue;

namespace ProcessManager
{
    public class ApplicationService
    {
        private readonly IList<Listener> _listeners = new List<Listener>();

        public ApplicationService(IMessageHandler messageHandler)
        {
            _listeners.Add(new Listener("ApplicationSubmission", messageHandler.StartApplicationMessageHandler));
            //_listeners.Add(new Listener("InternalCheckResponse", EventHandlers.InternalCheckResponseEventHandler));
            //_listeners.Add(new Listener("CreditCheckResponse", EventHandlers.CreditCheckResponseEventHandler));
            //_listeners.Add(new Listener("AccountOpenResponse", EventHandlers.AccountOpenResponseEventHandler));
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
