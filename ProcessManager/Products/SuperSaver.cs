using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using ProcessManager.Repositories;

namespace ProcessManager.Products
{
    public class SuperSaver : IProcessManager
    {
        private readonly Sender _sender;
        private readonly ApplyStream _applyStream;

        public SuperSaver()
        {
            _sender = new Sender();
            _applyStream = new ApplyStream();
        }

        public void ExecuteProcess(string eventName, string message, string messageId, string correlationId)
        {
            switch (eventName)
            {
                case "StartApplication":
                    _applyStream.Write("ApplicationStarted", message, ConstructMetaData(messageId, correlationId));
                    _sender.SendCommand("InternalCheckRequest", message, correlationId);
                    break;
                case "InternalCheckSuccess":
                    _applyStream.Write("InternalCheckSuccess", message, ConstructMetaData(messageId, correlationId));
                    _sender.SendCommand("CreditCheckRequest", message, correlationId);
                    break;
                case "CreditCheckSuccess":
                    _applyStream.Write("CreditCheckSuccess", message, ConstructMetaData(messageId, correlationId));
                    _sender.SendCommand("AccountOpenRequest", message, correlationId);
                    break;
                case "AccountOpenSuccess":
                    _applyStream.Write("AccountOpenSuccess", message, ConstructMetaData(messageId, correlationId));
                    _applyStream.Write("ApplicationCompleted", message, ConstructMetaData(messageId, correlationId));                 
                    break;
            }            
        }

        private string ConstructMetaData(string messageId, string correlationId)
        {
            return "{\"MessageId\": \"" + messageId + "\", \"CorrelationId\": \"" + correlationId + "\"}";
        }
    }
}
