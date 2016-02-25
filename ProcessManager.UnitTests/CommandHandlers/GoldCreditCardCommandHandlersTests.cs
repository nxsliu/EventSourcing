using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageQueue;
using NSubstitute;
using ProcessManager.CommandHandlers;
using ProcessManager.Commands;
using ProcessManager.Products;
using ProcessManager.Repositories;
using Xunit;

namespace ProcessManager.UnitTests.CommandHandlers
{
    public class GoldCreditCardCommandHandlersTests
    {
        [Fact]
        public void When_I_Handle_CreateGoldCreditCardApplication_the_correct_Command_is_sent()
        {
            var mockSender = Substitute.For<ISender>();
            var mockApplyStream = Substitute.For<IStreamRepository<GoldCreditCard>>();

            var goldCreditCardCommandHandlers = new GoldCreditCardCommandHandlers(mockSender, mockApplyStream);

            var applicationId = Guid.NewGuid();
            var messageId = Guid.NewGuid().ToString();
            var correlationId = Guid.NewGuid().ToString();

            goldCreditCardCommandHandlers.Handle(new CreateGoldCreditCardApplication(applicationId, "John", "jj@abc.com", 60000), messageId, correlationId);

            var expectedJson = "{\"Name\":\"John\",\"Email\":\"jj@abc.com\",\"AnnualIncome\":60000,\"InternalCheck\":false,\"Id\":\""+ applicationId + "\"}";

            mockSender.Received(1).SendCommand("InternalCheckRequest", expectedJson, correlationId);
        }
    }
}
