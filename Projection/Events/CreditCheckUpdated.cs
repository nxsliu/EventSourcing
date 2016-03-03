using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Events
{
    public class CreditCheckUpdated: Event
    {
        public bool CreditCheck { get; }

        public CreditCheckUpdated(Guid id, bool creditCheck) : base(id)
        {
            CreditCheck = creditCheck;
        }

        public override void Execute(IApplicationStatusRepository repo, int eventNumber)
        {
            repo.UpdateStatus(Id, CreditCheck ? "CreditCheckPassed" : "CreditCheckFailed", eventNumber);
        }
    }
}
