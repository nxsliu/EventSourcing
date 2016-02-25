using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Events
{
    public class Event
    {
    }

    public class GoldCreditCardCreated : Event
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Email;
        public readonly int AnnualIncome;

        public GoldCreditCardCreated(Guid id, string name, string email, int annualIncome)
        {
            Id = id;
            Name = name;
            Email = email;
            AnnualIncome = annualIncome;
        }
    }
}
