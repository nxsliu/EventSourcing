using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using EventStore;

namespace ProcessManager.Products
{
    public class SuperSaver : IProcessManager
    {
        private readonly Sender _sender;
        private readonly Writer _eventStoreWriter;

        public SuperSaver()
        {
            _sender = new Sender();
            _eventStoreWriter = new Writer();
        }

        public void ExecuteProcess(string eventName, string message, string correlationId)
        {
            var headers = new Dictionary<string, object>();
            headers.Add("ApplicationType", "SuperSaver");

            switch (eventName)
            {
                case "StartApplication":
                    _eventStoreWriter.Write("ApplicationStarted", message,
                        "{\"ApplicationType\": \"SuperSaver\", \"CorrelationId\": \"" + correlationId + "\"}");
                    _sender.SendCommand("Phase1Request", message, correlationId, headers);
                    break;
                case "Phase1Success":
                    _eventStoreWriter.Write("Phase1Success", message,
                        "{\"ApplicationType\": \"SuperSaver\", \"CorrelationId\": \"" + correlationId + "\"}");
                    _sender.SendCommand("Phase2Request", message, correlationId, headers);
                    break;
                case "Phase2Success":
                    _eventStoreWriter.Write("Phase2Success", message,
                        "{\"ApplicationType\": \"SuperSaver\", \"CorrelationId\": \"" + correlationId + "\"}");
                    _sender.SendCommand("Phase3Request", message, correlationId, headers);
                    break;
                case "Phase3Success":
                    _eventStoreWriter.Write("Phase3Success", message,
                        "{\"ApplicationType\": \"SuperSaver\", \"CorrelationId\": \"" + correlationId + "\"}");
                    _eventStoreWriter.Write("ApplicationCompleted", message,
                        "{\"ApplicationType\": \"SuperSaver\", \"CorrelationId\": \"" + correlationId + "\"}");
                    // finish the process                    
                    break;
            }            
        }
    }
}
