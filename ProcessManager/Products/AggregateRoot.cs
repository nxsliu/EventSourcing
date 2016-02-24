using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Events;
using Topshelf;

namespace ProcessManager.Products
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }    

        private readonly List<IEvent> _changes = new List<IEvent>();

        public IEnumerable<IEvent> GetUncommitedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void BuildFromHistory(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            ((dynamic)this).Apply(@event);

            if (isNew)
            {
                _changes.Add(@event);
            }
        }
    }
}
