using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using Newtonsoft.Json;
using ProcessManager.Commands;
using ProcessManager.Events;
using ProcessManager.Products;
using ProcessManager.Repositories;

namespace ProcessManager.CommandHandlers
{
    public class GoldCreditCardCommandHandlers: Dictionary<Type, Action<ICommand, string, string>>
    {
        private readonly ISender _sender;
        private readonly IStreamRepository<GoldCreditCard> _repository;

        public GoldCreditCardCommandHandlers(ISender sender, IStreamRepository<GoldCreditCard> repository)
        {
            this.Add(typeof(CreateGoldCreditCardApplication), (m, e, c) => Handle((CreateGoldCreditCardApplication)m, e, c));
            this.Add(typeof(UpdateInternalCheck), (m, e, c) => Handle((UpdateInternalCheck)m, e, c));
            this.Add(typeof(UpdateCreditCheck), (m, e, c) => Handle((UpdateCreditCheck)m, e, c));
            this.Add(typeof(UpdateAccountDetails), (m, e, c) => Handle((UpdateAccountDetails)m, e, c));

            _sender = sender;
            this._repository = repository;
        }

        public void Handle(CreateGoldCreditCardApplication message, string messageId, string correlationId)
        {
            var goldCreditCard = new GoldCreditCard(message.ApplicationId, message.Name, message.Email, message.AnnualIncome);            

            _repository.Save(goldCreditCard, new EventMetaData(messageId, correlationId));

            _sender.SendCommand("InternalCheckRequest", JsonConvert.SerializeObject(goldCreditCard), correlationId,
                new Dictionary<string, object>() {{"ApplicationType", "GoldCreditCard"}});
        }

        public void Handle(UpdateInternalCheck message, string messageId, string correlationId)
        {
            var goldCreditCard = _repository.GetById(message.ApplicationId);

            goldCreditCard.UpdateInternalCheck(message.InternalCheck);            

            _repository.Save(goldCreditCard, new EventMetaData(messageId, correlationId));

            if (message.InternalCheck)
            {
                _sender.SendCommand("CreditCheckRequest", JsonConvert.SerializeObject(goldCreditCard), correlationId,
                new Dictionary<string, object>() { { "ApplicationType", "GoldCreditCard" } });
            }
        }

        public void Handle(UpdateCreditCheck message, string messageId, string correlationId)
        {
            var goldCreditCard = _repository.GetById(message.ApplicationId);

            goldCreditCard.UpdateCreditCheck(message.CreditCheck);

            _repository.Save(goldCreditCard, new EventMetaData(messageId, correlationId));

            if (message.CreditCheck)
            {
                _sender.SendCommand("AccountOpenRequest", JsonConvert.SerializeObject(goldCreditCard), correlationId,
                new Dictionary<string, object>() { { "ApplicationType", "GoldCreditCard" } });
            }
        }

        public void Handle(UpdateAccountDetails message, string messageId, string correlationId)
        {
            if (message.AccountOpened)
            {
                var goldCreditCard = _repository.GetById(message.ApplicationId);
            
                goldCreditCard.UpdateAccountDetails(message.AccountNumber, message.BranchNumber);

                _repository.Save(goldCreditCard, new EventMetaData(messageId, correlationId));
            }
        }
    }
}
