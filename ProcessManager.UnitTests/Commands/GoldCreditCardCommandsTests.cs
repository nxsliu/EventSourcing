using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProcessManager.Commands;
using Xunit;

namespace ProcessManager.UnitTests.Commands
{
    public class GoldCreditCardCommandsTests
    {
        [Fact]
        public void When_I_call_CreateGoldCreditCardApplicationCommand_with_valid_json_correct_command_is_returned()
        {
            var command = new GoldCreditCardCommands();

            var jsonBody = new StringBuilder();
            jsonBody.Append("{");
            jsonBody.Append("\"ApplicationType\": \"GoldCreditCard\",");
            jsonBody.Append("\"ApplicationId\": \"307dbe42-e7b3-4034-88b8-2b677772cf64\",");
            jsonBody.Append("\"Name\": \"Matthew Freud\",");
            jsonBody.Append("\"AnnualIncome\":  50000,");
            jsonBody.Append("\"Email\":  \"mfreud@test123.com\"");
            jsonBody.Append("}");

            var result = command.CreateGoldCreditCardApplicationCommand(jsonBody.ToString()) as CreateGoldCreditCardApplication;

            result.Should().NotBeNull();
            result.ApplicationId.Should().Be(new Guid("307dbe42-e7b3-4034-88b8-2b677772cf64"));
            result.Name.Should().Be("Matthew Freud");
            result.AnnualIncome.Should().Be(50000);
            result.Email.Should().Be("mfreud@test123.com");
        }
    }
}
