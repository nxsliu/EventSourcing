using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager
{
    public interface IProcessManager
    {
        void ExecuteProcess(string command, string message, string messageId, string correlationId);
    }
}
