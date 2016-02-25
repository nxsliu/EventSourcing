using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.CommandHandlers;
using ProcessManager.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topshelf;

namespace ProcessManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var productCommands = new Dictionary<string, Dictionary<string, Func<string, ICommand>>>()
            {
                //{"SuperSaver", new SuperSaver()},
                {"GoldCreditCard", new GoldCreditCardCommands()},
                //{"NullProduct", new NullProduct()}
            };

            var productCommandHandlers = new Dictionary<string, Dictionary<Type, Action<ICommand, string, string>>>
            {
                {"GoldCreditCard", new GoldCreditCardCommandHandlers()},
            };

            var messageHandler = new MessageHandler(productCommands, productCommandHandlers);

            HostFactory.Run(x =>
            {
                x.Service<ApplicationService>(s =>
                {
                    s.ConstructUsing(name => new ApplicationService(messageHandler));
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
