using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace InternalCheckService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<CheckService>(s =>
                {
                    s.ConstructUsing(name => new CheckService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("EventSourcing POC");
                x.SetDisplayName("EventSourcing.InternalCheck");
                x.SetServiceName("EventSourcing.InternalCheck");
            });
        }
    }
}
