using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Models
{
    public class ApplicationViewModel
    {
        public Guid Id { get; }
        public string AccountType { get; }
        public string Name { get; }
        public string Email { get; }
        public string Status { get; }

        public ApplicationViewModel(Guid id, string accountType, string name, string email, string status)
        {
            this.Id = id;
            this.AccountType = accountType;
            this.Name = name;
            this.Email = email;
            this.Status = status;
        }
       
    }
}