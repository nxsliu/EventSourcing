using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topshelf;

namespace ProcessManager
{
    class Program
    {
        static void Main(string[] args)
        {   
            HostFactory.Run(x =>
            {
                x.Service<ApplicationService>(s =>
                {
                    s.ConstructUsing(name => new ApplicationService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("EventSourcing POC");
                x.SetDisplayName("EventSourcing.ProcessManager");
                x.SetServiceName("EventSourcing.ProcessManager");
            });
        }
    }
}
