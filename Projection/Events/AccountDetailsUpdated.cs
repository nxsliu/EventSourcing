using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Events
{
    public class AccountDetailsUpdated: Event
    {
        public string AccountNumber { get; }

        public string BranchNumber { get; }

        public AccountDetailsUpdated(Guid id, string accountNumber, string branchNumber) : base(id)
        {
            AccountNumber = accountNumber;
            BranchNumber = branchNumber;
        }

        public override void Execute(IApplicationStatusRepository repo, int eventNumber)
        {
            repo.UpdateAccountCreated(Id, AccountNumber, BranchNumber, eventNumber);            
        }
    }
}
