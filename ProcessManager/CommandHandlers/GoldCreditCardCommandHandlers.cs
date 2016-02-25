using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

            _sender = sender;
            this._repository = repository;
        }

        public void Handle(CreateGoldCreditCardApplication message, string messageId, string correlationId)
        {
            var goldCreditCard = new GoldCreditCard(message.ApplicationId, message.Name, message.Email, message.AnnualIncome);            

            _repository.Save(goldCreditCard, new EventMetaData(messageId, correlationId));

            _sender.SendCommand("InternalCheckRequest", JsonConvert.SerializeObject(goldCreditCard), correlationId);
        }

        public void Handle(UpdateInternalCheck message, string messageId, string correlationId)
        {
            var goldCreditCard = _repository.GetById(message.ApplicationId);

            goldCreditCard.UpdateInternalCheck(message.InternalCheck);

            _repository.Save(goldCreditCard, new EventMetaData(messageId, correlationId));
        }
    }
}
