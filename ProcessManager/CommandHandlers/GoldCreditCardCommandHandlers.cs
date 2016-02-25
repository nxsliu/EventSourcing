using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Commands;
using ProcessManager.Events;
using ProcessManager.Products;
using ProcessManager.Repositories;

namespace ProcessManager.CommandHandlers
{
    public class GoldCreditCardCommandHandlers: Dictionary<Type, Action<ICommand, string, string>>
    {
        private readonly ApplyStream<GoldCreditCard> _repository;

        public GoldCreditCardCommandHandlers()
        {
            this.Add(typeof(CreateGoldCreditCardApplication), (m, e, c) => Handle((CreateGoldCreditCardApplication)m, e, c));

            this._repository = new ApplyStream<GoldCreditCard>();
        }

        public void Handle(CreateGoldCreditCardApplication message, string messageId, string correlationId)
        {
            var goldCreditCard = new GoldCreditCard(message.ApplicationId, message.Name, message.Email, message.AnnualIncome);            

            _repository.Save(goldCreditCard, "ApplicationStarted", new EventMetaData(messageId, correlationId));
        }
    }
}
