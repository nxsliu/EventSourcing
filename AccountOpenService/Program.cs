using Topshelf;

namespace AccountOpenService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<AccountService>(s =>
                {
                    s.ConstructUsing(name => new AccountService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("EventSourcing POC");
                x.SetDisplayName("EventSourcing.AccountOpen");
                x.SetServiceName("EventSourcing.AccountOpen");
            });
        }
    }
}
