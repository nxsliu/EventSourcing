using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Events;

namespace ProcessManager.Products
{
    public class GoldCreditCard : AggregateRoot
    {        
        private string _name;
        private string _email;
        private int _annualIncome;

        private void Apply(GoldCreditCardCreated @event)
        {
            this.Id = @event.Id;
            this._name = @event.Name;
            this._email = @event.Email;
            this._annualIncome = @event.AnnualIncome;
        }

        public GoldCreditCard(Guid applicationId, string name, string email, int annualIncome)
        {
            ApplyChange(new GoldCreditCardCreated(applicationId, name, email, annualIncome));
        }
    }
}
