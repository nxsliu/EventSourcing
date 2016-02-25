using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountOpenService
{
    public class Application
    {
        public Guid Id { get; set; }
        public bool InternalCheck { get; set; }
        public bool CreditCheck { get; set; }
    }
}