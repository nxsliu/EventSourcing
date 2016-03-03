using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Events
{
    public class InternalCheckUpdated: Event
    {
        public bool InternalCheck { get; }

        public InternalCheckUpdated(Guid id, bool internalCheck) : base(id)
        {
            InternalCheck = internalCheck;
        }

        public override void Execute(IApplicationStatusRepository repo, int eventNumber)
        {
            repo.UpdateStatus(Id, InternalCheck ? "InternalCheckPassed" : "InternalCheckFailed", eventNumber);
        }
    }
}
