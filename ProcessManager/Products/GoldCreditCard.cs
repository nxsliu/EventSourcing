using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Products
{
    public class GoldCreditCard : IProcessManager
    {
        private string _applicationJson;

        public GoldCreditCard()
        {            
        }

        public GoldCreditCard(string applicationJson)
        {
            this._applicationJson = applicationJson;
        }

        public void ExecuteProcess(string command, string message, string messageId, string correlationId)
        {
            // execute phase 1
            // Send phase 1 command
            // Listen to phase 1 reply

            // If phase 1 reply == success then
            // execute phase A
            // Send phase A command
            // Listen to phase A reply

            // If phase A reply == success then
            // execute phase 3
            // Send phase 3 command
            // Listen to phase 3 reply

            // complete
            // Send complete event
        }
    }
}
