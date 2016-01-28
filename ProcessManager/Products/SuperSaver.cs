using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;

namespace ProcessManager.Products
{
    public class SuperSaver : IProcessManager
    {
        private string _applicationJson;
        private Sender _sender;

        public SuperSaver()
        {
            _sender = new Sender();
        }

        public SuperSaver(string applicationJson)
        {
            this._applicationJson = applicationJson;
        }

        public void ExecuteProcess(string eventName, string message, string correlationId)
        {
            var headers = new Dictionary<string, object>();
            headers.Add("ApplicationType", "SuperSaver");

            switch (eventName)
            {
                case "StartApplication":
                    _sender.SendCommand("Phase1Request", message, correlationId, headers);
                    break;
                case "Phase1Success":                    
                    _sender.SendCommand("Phase2Request", message, correlationId, headers);
                    break;
                case "Phase2Success":
                    _sender.SendCommand("Phase3Request", message, correlationId, headers);
                    break;
                case "Phase3Success":
                    // finish the process                    
                    break;
            }            
        }
    }
}
