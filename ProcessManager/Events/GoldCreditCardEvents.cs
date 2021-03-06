﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Events
{    
    public class GoldCreditCardCreated : Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int AnnualIncome { get; private set; }

        public GoldCreditCardCreated(Guid id, string name, string email, int annualIncome)
        {
            Id = id;
            Name = name;
            Email = email;
            AnnualIncome = annualIncome;
        }
    }

    public class InternalCheckUpdated : Event
    {
        public Guid Id { get; private set; }
        public bool InternalCheck { get; private set; }

        public InternalCheckUpdated(Guid id, bool internalCheck)
        {
            Id = id;
            InternalCheck = internalCheck;
        }
    }

    public class CreditCheckUpdated : Event
    {
        public Guid Id { get; private set; }
        public bool CreditCheck { get; private set; }

        public CreditCheckUpdated(Guid id, bool creditCheck)
        {
            Id = id;
            CreditCheck = creditCheck;
        }
    }

    public class AccountDetailsUpdated : Event
    {
        public Guid Id { get; private set; }
        public string AccountNumber { get; private set; }
        public string BranchNumber { get; private set; }

        public AccountDetailsUpdated(Guid id, string accountNumber, string branchNumber)
        {
            Id = id;
            AccountNumber = accountNumber;
            BranchNumber = branchNumber;
        }
    }
}
