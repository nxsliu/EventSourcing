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

        public ICommand CreateGoldCreditCardApplicationCommand(string data)
        {
            return JsonConvert.DeserializeObject<CreateGoldCreditCardApplication>(data);
        }
    }

    public interface ICommand
    {
    }

    public class CreateGoldCreditCardApplication : ICommand
    {       
        public Guid ApplicationId { get; private set; }
        public string Name { get; private set; }    
        public string Email { get; private set; }   
        public int AnnualIncome { get; private set; }

        public CreateGoldCreditCardApplication(Guid applicationId, string name, string email, int annualIncome)
        {
            ApplicationId = applicationId;
            Name = name;
            Email = email;
            AnnualIncome = annualIncome;
        }
    }
}
