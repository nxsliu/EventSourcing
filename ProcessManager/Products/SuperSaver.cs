using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using Newtonsoft.Json;
using ProcessManager.Events;
using ProcessManager.Repositories;

namespace ProcessManager.Products
{
    public class SuperSaver : AggregateRoot
    {
        [JsonProperty("Name")]
        private string _name;

        [JsonProperty("Email")]
        private string _email;        

        [JsonProperty("InternalCheck")]
        private bool? _internalCheck;        

        [JsonProperty("AccountNumber")]
        private string _accountNumber;

        [JsonProperty("BranchNumber")]
        private string _branchNumber;

        public SuperSaver()
        { }

        internal void Apply(SuperSaverCreated @event)
        {
            this.Id = @event.Id;
            this._name = @event.Name;
            this._email = @event.Email;
        }

        internal void Apply(InternalCheckUpdated @event)
        {
            this._internalCheck = @event.InternalCheck;
        }        

        internal void Apply(AccountDetailsUpdated @event)
        {
            this._accountNumber = @event.AccountNumber;
            this._branchNumber = @event.BranchNumber;
        }

        public SuperSaver(Guid applicationId, string name, string email)
        {
            ApplyChange(new SuperSaverCreated(applicationId, name, email));
        }

        public void UpdateInternalCheck(bool internalCheckStatus)
        {
            ApplyChange(new InternalCheckUpdated(Id, internalCheckStatus));
        }        

        public void UpdateAccountDetails(string accountNumber, string branchNumber)
        {
            ApplyChange(new AccountDetailsUpdated(Id, accountNumber, branchNumber));
        }
    }
}
