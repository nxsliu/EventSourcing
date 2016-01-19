using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Products
{
    public class SuperSaver : IProcessManager
    {
        private string _applicationJson;
        private MessageQueue _mq;

        public SuperSaver()
        {            
            _mq = new MessageQueue();
        }

        public SuperSaver(string applicationJson)
        {
            this._applicationJson = applicationJson;
        }

        public void ExecuteProcess(string eventName, string message, string correlationId)
        {
            switch (eventName)
            {
                case "StartApplication":
                _mq.SendCommand("Phase1Request", message, correlationId);
                    break;
            }

            // If phase 1 reply == success then
            // execute phase 2
            // Send phase 2 command
            // Listen to phase 2 reply

            // If phase 2 reply == success then
            // execute phase 3
            // Send phase 3 command
            // Listen to phase 3 reply

            // complete
            // Send complete event
        }
    }
}
