using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProcessManager.CommandHandlers;
using ProcessManager.Commands;
using ProcessManager.Products;
using RabbitMQ.Client.Events;

namespace ProcessManager
{
    public static class MessageHandler
    {
        private static readonly IDictionary<string, Dictionary<string, Func<string, ICommand>>> ProductCommands =
            new Dictionary<string, Dictionary<string, Func<string, ICommand>>>()
            {
                //{"SuperSaver", new SuperSaver()},
                {"GoldCreditCard", new GoldCreditCardCommands()},
                //{"NullProduct", new NullProduct()}
            };

        private static readonly IDictionary<string, Dictionary<Type, Action<ICommand>>> ProductCommandHandlers = 
            new Dictionary<string, Dictionary<Type, Action<ICommand>>>
            {
                {"GoldCreditCard", new GoldCreditCardCommandHandlers()},
            };

        public static void StartApplicationMessageHandler(object model, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);

            var application = JsonConvert.DeserializeObject<dynamic>(message);

            var productCommands = GetProductCommands((string)application.ApplicationType);

            var productCommandHandlers = GetProductCommandHandlers((string)application.ApplicationType);

            if (productCommands.ContainsKey("Create"))
            {
                var createCommand = productCommands["Create"](message);

                if (productCommandHandlers.ContainsKey(createCommand.GetType()))
                {
                    productCommandHandlers[createCommand.GetType()](createCommand);
                }
                else
                {
                    // log exception
                    // Commandhandler not found for Command for message
                }
            }
            else
            {
                // log exception
                // Command not found for message
            }
        }

        private static Dictionary<string, Func<string, ICommand>> GetProductCommands(string applicationType)
        {
            Dictionary<string, Func<string, ICommand>> productCommands;

            if (!ProductCommands.TryGetValue(applicationType, out productCommands))
            {
                ProductCommands.TryGetValue("NullProduct", out productCommands);
            }

            return productCommands;
        }

        private static Dictionary<Type, Action<ICommand>> GetProductCommandHandlers(string applicationType)
        {
            Dictionary<Type, Action<ICommand>> productCommandHandlers;

            if (!ProductCommandHandlers.TryGetValue(applicationType, out productCommandHandlers))
            {
                ProductCommandHandlers.TryGetValue("NullProduct", out productCommandHandlers);
            }

            return productCommandHandlers;
        }
    }
}
