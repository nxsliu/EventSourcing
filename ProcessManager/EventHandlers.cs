using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Products;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace ProcessManager
{
    public static class EventHandlers
    {
        private static readonly IDictionary<string, IProcessManager> _processManagers = new Dictionary<string, IProcessManager>();

        public static void StartApplicationEventHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleEvent(ea, "StartApplication");
        }

        public static void InternalCheckResponseEventHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleEvent(ea, "InternalCheckSuccess");
        }

        public static void CreditCheckResponseEventHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleEvent(ea, "CreditCheckSuccess");            
        }

        public static void AccountOpenResponseEventHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleEvent(ea, "AccountOpenSuccess");
        }

        private static void HandleEvent(BasicDeliverEventArgs ea, string eventName)
        {
            var message = Encoding.UTF8.GetString(ea.Body);

            var application = JsonConvert.DeserializeObject<dynamic>(message);

            var product = GetProcessManager((string)application.ApplicationType);

            product.ExecuteProcess(eventName, message, ea.BasicProperties.MessageId, ea.BasicProperties.CorrelationId);
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
                    if (_processManagers.TryGetValue("NullProduct", out processManager)) return processManager;
                    
                    processManager = new NullProduct();
                    _processManagers.Add("NullProduct", processManager);
                    return processManager;
            }

            _processManagers.Add(applicationType, processManager);
            return processManager;
        }
    }
}
