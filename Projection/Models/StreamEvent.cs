using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Projection.Events;
using Projection.Repositories;

namespace Projection.Models
{
    public class EventWrapper
    {
        public string EventId { get; private set ; }

        public string EventType { get; private set; }

        public int EventNumber { get; private set; }

        public JObject Data { get; private set; }

        public EventWrapper(string eventId, string eventType, int eventNumber, JObject data)
        {
            EventId = eventId;
            EventType = eventType;
            EventNumber = eventNumber;
            Data = data;
        }
    }

    public class StreamEvent
    {
        public EventWrapper Content { get; set; }        
    }
}
