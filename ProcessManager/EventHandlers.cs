using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Products;
using RabbitMQ.Client.Events;

namespace ProcessManager
{
    public static class EventHandlers
    {
        private static readonly IDictionary<string, IProcessManager> _processManagers = new Dictionary<string, IProcessManager>();

        public static void StartApplicationEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            object header;
            if (!ea.BasicProperties.Headers.TryGetValue("ApplicationType", out header))
            {
                // raises error
            }

            var message = Encoding.UTF8.GetString(body);
            var applicationType = Encoding.UTF8.GetString((byte[])header);

            var product = GetProcessManager(applicationType);

            product.ExecuteProcess("StartApplication", message, ea.BasicProperties.CorrelationId);
        }

        public static void Phase1ResponseEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            object header;
            if (!ea.BasicProperties.Headers.TryGetValue("ApplicationType", out header))
            {
                // raises error
            }

            var message = Encoding.UTF8.GetString(body);
            var applicationType = Encoding.UTF8.GetString((byte[])header);

            var product = GetProcessManager(applicationType);

            product.ExecuteProcess("Phase1Response", message, ea.BasicProperties.CorrelationId);
        }

        public static void Phase2ResponseEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            object header;
            if (!ea.BasicProperties.Headers.TryGetValue("ApplicationType", out header))
            {
                // raises error
            }

            var message = Encoding.UTF8.GetString(body);
            var applicationType = Encoding.UTF8.GetString((byte[])header);

            var product = GetProcessManager(applicationType);

            product.ExecuteProcess("Phase2Response", message, ea.BasicProperties.CorrelationId);
        }

        public static void Phase3ResponseEventHandler(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            object header;
            if (!ea.BasicProperties.Headers.TryGetValue("ApplicationType", out header))
            {
                // raises error
            }

            var message = Encoding.UTF8.GetString(body);
            var applicationType = Encoding.UTF8.GetString((byte[])header);

            var product = GetProcessManager(applicationType);

            product.ExecuteProcess("Phase3Response", message, ea.BasicProperties.CorrelationId);
        }

        private static IProcessManager GetProcessManager(string applicationType)
        {
            IProcessManager processManager;

            if (_processManagers.TryGetValue(applicationType, out processManager)) return processManager;

            switch (applicationType)
            {
                case "SuperSaver":
                    processManager = new SuperSaver();
                    break;
                case "GoldCreditCard":
                    processManager = new GoldCreditCard();
                    break;
                default:
                    // this is buggy, everytime an unrecognised applicationType is passed
                    // a NullProduct will be added
                    processManager = new NullProduct();
                    break;
            }

            _processManagers.Add(applicationType, processManager);
            return processManager;
        }
    }
}
