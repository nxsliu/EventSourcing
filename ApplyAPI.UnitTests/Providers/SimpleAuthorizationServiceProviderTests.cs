using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplyAPI.UnitTests.Providers
{
    public class SimpleAuthorizationServiceProviderTests
    {
        [Fact]
        public void Getting_Hash_Secret()
        {
            var result = Helper.GetHash("Ev3ntS@urc3");
        }
    }
}
