using Topshelf;

namespace CreditCheckService
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
                x.SetDisplayName("EventSourcing.CreditCheck");
                x.SetServiceName("EventSourcing.CreditCheck");
            });
        }
    }
}
