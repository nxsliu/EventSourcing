using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Events
{
    public class SuperSaverCreated: Event
    {
        public string Name { get; }

        public string Email { get; }

        public SuperSaverCreated(Guid id, string name, string email) : base(id)
        {
            Name = name;
            Email = email;
        }

        public override void Execute(IApplicationStatusRepository repo, int eventNumber)
        {
            repo.CreateSuperSaverApply(Id, Name, Email, eventNumber);
        }
    }
}
