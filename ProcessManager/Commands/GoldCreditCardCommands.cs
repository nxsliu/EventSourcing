using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProcessManager.Commands
{
    public class GoldCreditCardCommands: Dictionary<string, Func<string, ICommand>>
    {
        public GoldCreditCardCommands()
        {
            this.Add("Create", CreateGoldCreditCardApplicationCommand);
        }

        private static ICommand CreateGoldCreditCardApplicationCommand(string data)
        {
            return JsonConvert.DeserializeObject<CreateGoldCreditCardApplication>(data);
        }
    }

    public interface ICommand
    {
    }

    public class CreateGoldCreditCardApplication : ICommand
    {
        public readonly Guid ApplicationId;
        public readonly string Name;
        public readonly string Email;
        public readonly int AnnualIncome;        
    }
}
