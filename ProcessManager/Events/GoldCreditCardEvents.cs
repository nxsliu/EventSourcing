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
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int AnnualIncome { get; private set; }

        public GoldCreditCardCreated(Guid id, string name, string email, int annualIncome)
        {
            Id = id;
            Name = name;
            Email = email;
            AnnualIncome = annualIncome;
        }
    }

    public class InternalCheckUpdated : Event
    {
        public Guid Id { get; private set; }
        public bool InternalCheck { get; private set; }

        public InternalCheckUpdated(Guid id, bool internalCheck)
        {
            Id = id;
            InternalCheck = internalCheck;
        }
    }
}
