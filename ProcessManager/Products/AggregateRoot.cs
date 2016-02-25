using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Events;

namespace ProcessManager.Products
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }    

        private readonly List<Event> _changes = new List<Event>();

        public IEnumerable<Event> GetUncommitedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void BuildFromHistory(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            ((dynamic)this).Apply((dynamic)@event);

            if (isNew)
            {
                _changes.Add(@event);
            }
        }
    }
}
