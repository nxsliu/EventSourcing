using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Events
{
    public class GoldCreditCardCreated : Event
    {
        public string Name { get; }

        public string Email { get; }

        public int AnnualIncome { get; }

        public GoldCreditCardCreated(Guid id, string name, string email, int annualIncome) : base(id)
        {
            Name = name;
            Email = email;
            AnnualIncome = annualIncome;
        }

        public override void Execute(IApplicationStatusRepository repo, int eventNumber)
        {
            repo.CreateGoldCreditCardApply(Id, Name, Email, AnnualIncome, eventNumber);
        }
    }
}
