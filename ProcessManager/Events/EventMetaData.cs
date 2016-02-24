using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Events
{
    public class EventMetaData
    {
        public string MessageId { get; private set; }  
        public string CorrelationId { get; private set; }

        public EventMetaData(string messageId, string correlationId)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
        }
    }
}
