using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Events
{
    public class SuperSaverCreated : Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public SuperSaverCreated(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
