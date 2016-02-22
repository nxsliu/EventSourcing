using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Products
{
    public class NullProduct : IProcessManager
    {
        public void ExecuteProcess(string command, string message, string messageId, string correlationId)
        {
            // do nothing
        }
    }
}
