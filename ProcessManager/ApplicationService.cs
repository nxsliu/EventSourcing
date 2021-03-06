﻿using System;
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
            _listeners.Add(new Listener("InternalCheckResponse", messageHandler.InternalCheckResponseMessageHandler));
            _listeners.Add(new Listener("CreditCheckResponse", messageHandler.CreditCheckResponseMessageHandler));
            _listeners.Add(new Listener("AccountOpenResponse", messageHandler.AccountOpenResponseMessageHandler));
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
