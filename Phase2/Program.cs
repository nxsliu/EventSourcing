using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Phase2
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Phase2Service>(s =>
                {
                    s.ConstructUsing(name => new Phase2Service());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("EventSourcing POC");
                x.SetDisplayName("EventSourcing.Phase2");
                x.SetServiceName("EventSourcing.Phase2");
            });
        }
    }
}
