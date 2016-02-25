using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProcessManager.Products;
using Xunit;

namespace ProcessManager.UnitTests.Products
{    
    public class GoldCreditCardTests
    {
        [Fact]
        public void When_I_call_ApplyChange_the_correct_child_Apply_is_called()
        {
            var id = Guid.NewGuid();

            var goldCreditCard = new GoldCreditCard(id, "John", "jj@abc.com", 5000);

            goldCreditCard.Id.Should().Be(id);
        }
    }
}
