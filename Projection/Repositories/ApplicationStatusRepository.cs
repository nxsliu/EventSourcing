using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using Projection.Models;
using Dapper;

namespace Projection.Repositories
{
    public interface IApplicationStatusRepository
    {
        bool ApplicationExists(Guid applicationId);

        void CreateSuperSaverApply(Guid id, string name, string email, int eventNumber);

        void CreateGoldCreditCardApply(Guid id, string name, string email, int annualIncome, int eventNumber);

        void UpdateAccountCreated(Guid id, string accountNumber, string branchNumber, int eventNumber);

        void UpdateStatus(Guid id, string status, int eventNumber);        

        int? GetLastSuccessfulEvent();
    }

    public class ApplicationStatusRepository : IApplicationStatusRepository
    {
        private const string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\EventSourcing\Projection\Database\ApplicationAdmin.mdf;Integrated Security=True";        

        public bool ApplicationExists(Guid applicationId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    var count = conn.Query<int>("SELECT COUNT(Id) FROM Apply WHERE Id = @Id", new { Id = applicationId }).Single();

                    return count > 0;
                }
                catch (Exception)
                {                    
                    throw;
                }                
            }
        }

        public void CreateSuperSaverApply(Guid id, string name, string email, int eventNumber)
        {
            if (ApplicationExists(id))
            {
                // something is wrong
                return;
            }

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var query = new StringBuilder();
                    query.AppendLine("BEGIN TRAN");
                    query.AppendLine(
                        "INSERT INTO Apply (Id, AccountType, Name, Email, Status) VALUES (@Id, @AccountType, @Name, @Email, @Status)");
                    query.AppendLine("INSERT INTO SuperSaver (ApplyId) VALUES (@Id)");
                    query.AppendLine("UPDATE LastSuccessfulEvent SET EventNumber = @EventNumber");
                    query.AppendLine("COMMIT");

                    conn.Open();
                    conn.Execute(query.ToString(),
                        new
                        {
                            Id = id,
                            AccountType = "SuperSaver",
                            Name = name,
                            Email = email,
                            Status = "ApplicationStarted",
                            EventNumber = eventNumber
                        });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateGoldCreditCardApply(Guid id, string name, string email, int annualIncome, int eventNumber)
        {
            if (ApplicationExists(id))
            {
                // something is wrong
                return;
            }

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var query = new StringBuilder();
                    query.AppendLine("BEGIN TRAN");
                    query.AppendLine("INSERT INTO Apply (Id, AccountType, Name, Email, Status) VALUES (@Id, @AccountType, @Name, @Email, @Status)");
                    query.AppendLine("INSERT INTO GoldCreditCard (ApplyId, AnnualIncome) VALUES (@Id, @AnnualIncome)");
                    query.AppendLine("UPDATE LastSuccessfulEvent SET EventNumber = @EventNumber");
                    query.AppendLine("COMMIT");

                    conn.Open();
                    conn.Execute(query.ToString(),
                        new
                        {
                            Id = id,
                            AccountType = "GoldCreditCard",
                            Name = name,
                            Email = email,
                            AnnualIncome = annualIncome,
                            Status = "ApplicationStarted",
                            EventNumber = eventNumber
                        });
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void UpdateAccountCreated(Guid id, string accountNumber, string branchNumber, int eventNumber)
        {
            var accountType = GetAccountType(id);            

            using (var conn = new SqlConnection(ConnectionString))
            {
                var query = new StringBuilder();
                query.AppendLine("BEGIN TRAN");
                query.AppendLine($"UPDATE {accountType} SET AccountNumber = @AccountNumber, BranchNumber = @BranchNumber WHERE ApplyId = @Id");
                query.AppendLine("UPDATE Apply SET Status = @Status WHERE Id = @Id");
                query.AppendLine("INSERT INTO ApplyNote (ApplyId, Note, DateCreated) VALUES (@Id, @Note, @DateCreated)");
                query.AppendLine("UPDATE LastSuccessfulEvent SET EventNumber = @EventNumber");
                query.AppendLine("COMMIT");

                try
                {
                    conn.Open();
                    conn.Execute(query.ToString(),
                        new
                        {
                            Id = id,
                            AccountNumber = accountType,
                            BranchNumber = branchNumber,
                            Status = "ApplicationEnded",
                            Note = "Application process ended",
                            DateCreated = DateTime.UtcNow,
                            EventNumber = eventNumber
                        });
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public string GetAccountType(Guid id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    return conn.Query<string>("SELECT AccountType FROM Apply WHERE Id = @Id", new { Id = id }).Single();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void UpdateStatus(Guid id, string status, int eventNumber)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var query = new StringBuilder();
                query.AppendLine("BEGIN TRAN");
                query.AppendLine("UPDATE Apply SET Status = @Status WHERE Id = @Id");
                query.AppendLine("INSERT INTO ApplyNote (ApplyId, Note, DateCreated) VALUES (@Id, @Note, @DateCreated)");
                query.AppendLine("UPDATE LastSuccessfulEvent SET EventNumber = @EventNumber");
                query.AppendLine("COMMIT");

                try
                {
                    conn.Open();
                    conn.Execute(query.ToString(),
                        new
                        {
                            Id = id,
                            Status = status,
                            Note = $"Application status updated to {status}",
                            DateCreated = DateTime.UtcNow,
                            EventNumber = eventNumber
                        });
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public int? GetLastSuccessfulEvent()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    return conn.Query<int?>("SELECT EventNumber FROM LastSuccessfulEvent").Single();
                }
                catch (Exception)
                {                    
                    throw;
                }                
            }
        }
    }
}
