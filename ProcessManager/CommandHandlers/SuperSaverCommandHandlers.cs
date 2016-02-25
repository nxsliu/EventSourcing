using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using Newtonsoft.Json;
using ProcessManager.Commands;
using ProcessManager.Events;
using ProcessManager.Products;
using ProcessManager.Repositories;
using Topshelf.HostConfigurators.AssemblyExtensions;

namespace ProcessManager.CommandHandlers
{
    public class SuperSaverCommandHandlers : Dictionary<Type, Action<ICommand, string, string>>
    {
        private readonly ISender _sender;
        private readonly IStreamRepository<SuperSaver> _repository;

        public SuperSaverCommandHandlers(ISender sender, IStreamRepository<SuperSaver> repository)
        {
            this.Add(typeof(CreateSuperSaverApplication), (m, e, c) => Handle((CreateSuperSaverApplication)m, e, c));
            this.Add(typeof(UpdateInternalCheck), (m, e, c) => Handle((UpdateInternalCheck)m, e, c));
            this.Add(typeof(UpdateAccountDetails), (m, e, c) => Handle((UpdateAccountDetails)m, e, c));

            _sender = sender;
            this._repository = repository;
        }

        public void Handle(CreateSuperSaverApplication message, string messageId, string correlationId)
        {
            var superSaver = new SuperSaver(message.ApplicationId, message.Name, message.Email);

            _repository.Save(superSaver, new EventMetaData(messageId, correlationId));

            _sender.SendCommand("InternalCheckRequest", JsonConvert.SerializeObject(superSaver), correlationId,
                new Dictionary<string, object>() { { "ApplicationType", "SuperSaver" } });
        }

        public void Handle(UpdateInternalCheck message, string messageId, string correlationId)
        {
            var superSaver = _repository.GetById(message.ApplicationId);

            superSaver.UpdateInternalCheck(message.InternalCheck);

            _repository.Save(superSaver, new EventMetaData(messageId, correlationId));

            if (message.InternalCheck)
            {
                _sender.SendCommand("AccountOpenRequest", JsonConvert.SerializeObject(superSaver), correlationId,
                new Dictionary<string, object>() { { "ApplicationType", "SuperSaver" } });
            }
        }        

        public void Handle(UpdateAccountDetails message, string messageId, string correlationId)
        {
            if (message.AccountOpened)
            {
                var superSaver = _repository.GetById(message.ApplicationId);

                superSaver.UpdateAccountDetails(message.AccountNumber, message.BranchNumber);

                _repository.Save(superSaver, new EventMetaData(messageId, correlationId));
            }
        }
    }
}
