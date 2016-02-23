using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManager.Commands;

namespace ProcessManager.CommandHandlers
{
    public class GoldCreditCardCommandHandlers: Dictionary<Type, Action<ICommand>>
    {
        public GoldCreditCardCommandHandlers()
        {
            this.Add(typeof(CreateGoldCreditCardApplication), m => Handle((CreateGoldCreditCardApplication)m));
        }

        public void Handle(CreateGoldCreditCardApplication message)
        {
            // do something
        }
    }
}
