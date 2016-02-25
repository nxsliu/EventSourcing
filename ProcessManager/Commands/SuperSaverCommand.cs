using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProcessManager.Commands
{
    public class SuperSaverCommands : Dictionary<string, Func<string, ICommand>>
    {
        public SuperSaverCommands()
        {
            this.Add("Create", CreateSuperSaverApplicationCommand);
            this.Add("UpdateInternalCheck", CreateUpdateInternalCheckCommand);
            this.Add("UpdateAccountDetails", CreateUpdateAccountDetailsCommand);
        }

        public ICommand CreateSuperSaverApplicationCommand(string data)
        {
            return JsonConvert.DeserializeObject<CreateSuperSaverApplication>(data);
        }

        public ICommand CreateUpdateInternalCheckCommand(string data)
        {
            return JsonConvert.DeserializeObject<UpdateInternalCheck>(data);
        }        

        public ICommand CreateUpdateAccountDetailsCommand(string data)
        {
            return JsonConvert.DeserializeObject<UpdateAccountDetails>(data);
        }
    }    

    public class CreateSuperSaverApplication : ICommand
    {
        public Guid ApplicationId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public CreateSuperSaverApplication(Guid applicationId, string name, string email)
        {
            ApplicationId = applicationId;
            Name = name;
            Email = email;
        }
    }    
}
