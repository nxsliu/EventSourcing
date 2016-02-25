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
    public interface IMessageHandler
    {
        void StartApplicationMessageHandler(object model, BasicDeliverEventArgs ea);
        void InternalCheckResponseMessageHandler(object model, BasicDeliverEventArgs ea);
        void CreditCheckResponseMessageHandler(object model, BasicDeliverEventArgs ea);
        void AccountOpenResponseMessageHandler(object model, BasicDeliverEventArgs ea);
    }

    public class MessageHandler : IMessageHandler
    {
        private readonly IDictionary<string, Dictionary<string, Func<string, ICommand>>> _productCommands;

        private readonly IDictionary<string, Dictionary<Type, Action<ICommand, string, string>>> _productCommandHandlers;

        public MessageHandler(IDictionary<string, Dictionary<string, Func<string, ICommand>>> productCommands,
            IDictionary<string, Dictionary<Type, Action<ICommand, string, string>>> productCommandHandlers)
        {
            this._productCommands = productCommands;
            this._productCommandHandlers = productCommandHandlers;
        }

        public void StartApplicationMessageHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleMessage(ea, "Create");
        }

        public void InternalCheckResponseMessageHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleMessage(ea, "UpdateInternalCheck");
        }

        public void CreditCheckResponseMessageHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleMessage(ea, "UpdateCreditCheck");
        }

        public void AccountOpenResponseMessageHandler(object model, BasicDeliverEventArgs ea)
        {
            HandleMessage(ea, "UpdateAccountDetails");
        }

        private void HandleMessage(BasicDeliverEventArgs ea, string commandKey)
        {
            var message = Encoding.UTF8.GetString(ea.Body);

            var applicationType = Encoding.UTF8.GetString((byte[]) ea.BasicProperties.Headers["ApplicationType"]);

            var productCommands = GetProductCommands(applicationType);

            var productCommandHandlers = GetProductCommandHandlers(applicationType);

            if (productCommands.ContainsKey(commandKey))
            {
                var command = productCommands[commandKey](message);

                if (productCommandHandlers.ContainsKey(command.GetType()))
                {
                    productCommandHandlers[command.GetType()](command, ea.BasicProperties.MessageId,
                        ea.BasicProperties.CorrelationId);
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

        private IDictionary<string, Func<string, ICommand>> GetProductCommands(string applicationType)
        {
            Dictionary<string, Func<string, ICommand>> productCommands;

            if (!_productCommands.TryGetValue(applicationType, out productCommands))
            {
                _productCommands.TryGetValue("NullProduct", out productCommands);
            }

            return productCommands;
        }

        private IDictionary<Type, Action<ICommand, string, string>> GetProductCommandHandlers(string applicationType)
        {
            Dictionary<Type, Action<ICommand, string, string>> productCommandHandlers;

            if (!_productCommandHandlers.TryGetValue(applicationType, out productCommandHandlers))
            {
                _productCommandHandlers.TryGetValue("NullProduct", out productCommandHandlers);
            }

            return productCommandHandlers;
        }
    }
}
