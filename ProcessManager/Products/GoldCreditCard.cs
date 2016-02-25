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
        private bool _internalCheck;

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

        public GoldCreditCard(Guid applicationId, string name, string email, int annualIncome)
        {
            ApplyChange(new GoldCreditCardCreated(applicationId, name, email, annualIncome));
        }

        public void UpdateInternalCheck(bool internalCheckStatus)
        {
            ApplyChange(new InternalCheckUpdated(Id, internalCheckStatus));
        }
    }
}
