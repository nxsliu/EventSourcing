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
        bool ApplicationExists(string applicationId);

        void CreateApplication(ApplicationContent application);

        void UpdateApplication(ApplicationContent application);

        int? GetLastSuccessfulEvent();

    }

    public class ApplicationStatusRepository : IApplicationStatusRepository
    {
        private const string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects POC\EventSourcing\Projection\Database\ApplicationAdmin.mdf;Integrated Security=True";        

        public bool ApplicationExists(string applicationId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var count = conn.Query<int>("SELECT COUNT(Id) FROM ApplicationStatus WHERE Id = @Id", new {Id = applicationId}).Single();

                return count > 0;
            }
        }

        public void CreateApplication(ApplicationContent application)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var query = new StringBuilder();
                query.AppendLine("BEGIN TRAN");
                query.AppendLine("INSERT INTO ApplicationStatus (Id, Name, Status, DateModified, DateCreated) VALUES (@Id, @Name, @Status, @DateCreated, @DateCreated)");
                query.AppendLine("UPDATE LastSuccessfulEvent SET EventNumber = @EventNumber");
                query.AppendLine("COMMIT");

                conn.Open();
                conn.Execute(query.ToString(),
                    new
                    {
                        Id = application.Data.ApplicationId,
                        Name = application.Data.Name,
                        Status = application.EventType,
                        DateCreated = DateTime.UtcNow,
                        EventNumber = application.EventNumber
                    });
            }
        }

        public void UpdateApplication(ApplicationContent application)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var query = new StringBuilder();
                query.AppendLine("BEGIN TRAN");
                query.AppendLine("UPDATE ApplicationStatus SET Status = @Status, DateModified = @DateModified WHERE Id = @Id");
                query.AppendLine("UPDATE LastSuccessfulEvent SET EventNumber = @EventNumber");
                query.AppendLine("COMMIT");

                conn.Open();
                conn.Execute(query.ToString(),
                    new
                    {
                        Id = application.Data.ApplicationId,
                        Status = application.EventType,
                        DateModified = DateTime.UtcNow,
                        EventNumber = application.EventNumber
                    });
            }
        }

        public int? GetLastSuccessfulEvent()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<int?>("SELECT EventNumber FROM LastSuccessfulEvent").Single();
            }
        }
    }
}
