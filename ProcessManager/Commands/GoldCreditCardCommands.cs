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
            this.Add("UpdateInternalCheck", CreateUpdateInternalCheckCommand);
            this.Add("UpdateCreditCheck", CreateUpdateCreditCheckCommand);
            this.Add("UpdateAccountDetails", CreateUpdateAccountDetailsCommand);
        }

        public ICommand CreateGoldCreditCardApplicationCommand(string data) 
        {
            return JsonConvert.DeserializeObject<CreateGoldCreditCardApplication>(data);
        }

        public ICommand CreateUpdateInternalCheckCommand(string data)
        {
            return JsonConvert.DeserializeObject<UpdateInternalCheck>(data);
        }

        public ICommand CreateUpdateCreditCheckCommand(string data)
        {
            return JsonConvert.DeserializeObject<UpdateCreditCheck>(data);
        }

        public ICommand CreateUpdateAccountDetailsCommand(string data)
        {
            return JsonConvert.DeserializeObject<UpdateAccountDetails>(data);
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

    public class UpdateInternalCheck : ICommand
    {
        public Guid ApplicationId { get; private set; }
        public bool InternalCheck { get; private set; }

        public UpdateInternalCheck(Guid applicationId, bool internalCheck)
        {
            ApplicationId = applicationId;
            InternalCheck = internalCheck;
        }
    }

    public class UpdateCreditCheck : ICommand
    {
        public Guid ApplicationId { get; private set; }
        public bool CreditCheck { get; private set; }

        public UpdateCreditCheck(Guid applicationId, bool creditCheck)
        {
            ApplicationId = applicationId;
            CreditCheck = creditCheck;
        }
    }

    public class UpdateAccountDetails : ICommand
    {
        public Guid ApplicationId { get; private set; }
        public string AccountNumber { get; private set; }
        public string BranchNumber { get; private set; }
        public bool AccountOpened { get; private set; }

        public UpdateAccountDetails(Guid applicationId, string accountNumber, string branchNumber, bool accountOpened)
        {
            ApplicationId = applicationId;
            AccountNumber = accountNumber;
            BranchNumber = branchNumber;
            AccountOpened = accountOpened;
        }
    }
}
