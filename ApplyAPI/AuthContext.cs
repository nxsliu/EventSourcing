using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplyAPI.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ApplyAPI
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("AuthContext")
        {
            Database.SetInitializer<AuthContext>(new DropCreateDatabaseIfModelChanges<AuthContext>());
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
