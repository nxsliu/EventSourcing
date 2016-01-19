using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProcessManager.Events;
using ProcessManager.Products;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProcessManager
{
    public class ApplicationService
    {
        private readonly IDictionary<string, IProcessManager> _processManagers = new Dictionary<string, IProcessManager>();
        private readonly IList<QueueListener> _listeners = new List<QueueListener>();

        public ApplicationService()
        {
            _listeners.Add(new QueueListener("ApplicationSubmission", EventHandlers.StartApplicationEventHandler));
            _listeners.Add(new QueueListener("Phase1Response", EventHandlers.Phase1ResponseEventHandler));
            //_listeners.Add(new QueueListener("Phase2Response", EventHandlers.Phase2ResponseEventHandler));
            //_listeners.Add(new QueueListener("Phase3Response", EventHandlers.Phase3ResponseEventHandler));
        }

        public void Start()
        {           
            foreach (var listener in _listeners)
            {
                listener.Start();
            }           
        }

        public void Stop()
        {
            foreach (var listener in _listeners)
            {
                listener.Stop();
            }
        }

        //private void EventHandler(string name, object model, BasicDeliverEventArgs ea)
        //{
        //    var body = ea.Body;
        //    object header;
        //    if (!ea.BasicProperties.Headers.TryGetValue("ApplicationType", out header))
        //    {
        //        // raises error
        //    }

        //    var message = Encoding.UTF8.GetString(body);
        //    var applicationType = Encoding.UTF8.GetString((byte[]) header);                           

        //    var product = GetProcessManager(applicationType);

        //    product.ExecuteProcess("StartApplication", message, ea.BasicProperties.CorrelationId);
        //}

        //private IProcessManager GetProcessManager(string applicationType)
        //{
        //    IProcessManager processManager;

        //    if (_processManagers.TryGetValue(applicationType, out processManager)) return processManager;

        //    switch (applicationType)
        //    {
        //        case "SuperSaver":
        //            processManager = new SuperSaver();
        //            break;
        //        case "GoldCreditCard":
        //            processManager = new GoldCreditCard();
        //            break;
        //        default:
        //            processManager = new NullProduct();
        //            break;
        //    }

        //    _processManagers.Add(applicationType, processManager);
        //    return processManager;
        //}
    }
}
