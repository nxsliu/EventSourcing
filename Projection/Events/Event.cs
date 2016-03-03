using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Events
{
    public interface IEvent
    {
        void Execute(IApplicationStatusRepository repo, int eventNumber);
    }

    public abstract class Event : IEvent
    {
        public Guid Id { get; }

        protected Event(Guid id)
        {
            Id = id;
        }

        public abstract void Execute(IApplicationStatusRepository repo, int eventNumber);
    }
}
