using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProcessManager.Events;

namespace ProcessManager.Products
{
    public class GoldCreditCard : AggregateRoot
    {        
        [JsonProperty("Name")]
        private string _name;

        [JsonProperty("Email")]
        private string _email;

        [JsonProperty("AnnualIncome")]
        private int _annualIncome;

        [JsonProperty("InternalCheck")]
        private bool? _internalCheck;

        [JsonProperty("CreditCheck")]
        private bool? _creditCheck;

        [JsonProperty("AccountNumber")]
        private string _accountNumber;

        [JsonProperty("BranchNumber")]
        private string _branchNumber;

        public GoldCreditCard()
        { }

        internal void Apply(GoldCreditCardCreated @event)
        {
            this.Id = @event.Id;
            this._name = @event.Name;
            this._email = @event.Email;
            this._annualIncome = @event.AnnualIncome;
        }

        internal void Apply(InternalCheckUpdated @event)
        {
            this._internalCheck = @event.InternalCheck;
        }

        internal void Apply(CreditCheckUpdated @event)
        {
            this._creditCheck = @event.CreditCheck;
        }

        internal void Apply(AccountDetailsUpdated @event)
        {
            this._accountNumber = @event.AccountNumber;
            this._branchNumber = @event.BranchNumber;
        }

        public GoldCreditCard(Guid applicationId, string name, string email, int annualIncome)
        {
            ApplyChange(new GoldCreditCardCreated(applicationId, name, email, annualIncome));
        }

        public void UpdateInternalCheck(bool internalCheckStatus)
        {
            ApplyChange(new InternalCheckUpdated(Id, internalCheckStatus));
        }

        public void UpdateCreditCheck(bool creditCheckStatus)
        {
            ApplyChange(new CreditCheckUpdated(Id, creditCheckStatus));
        }

        public void UpdateAccountDetails(string accountNumber, string branchNumber)
        {
            ApplyChange(new AccountDetailsUpdated(Id, accountNumber, branchNumber));
        }
    }
}
