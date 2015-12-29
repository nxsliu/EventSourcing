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
    }

    public class ApplicationStatusRepository : IApplicationStatusRepository
    {
        public bool ApplicationExists(string applicationId)
        {
            using (var conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\ApplicationAdmin.mdf;Integrated Security=True"))
            {
                conn.Open();
                var count = conn.Query<int>("select count(Id) from ApplicationStatus where Id = @Id", new {Id = applicationId}).Single();

                return count > 0;
            }
        }

        public void CreateApplication(ApplicationContent application)
        {
            using (var conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\ApplicationAdmin.mdf;Integrated Security=True"))
            {
                conn.Open();
                conn.Execute(
                    "insert into ApplicationStatus (Id, Name, Status, DateModified, DateCreated) values (@Id, @Name, @Status, @DateCreated, @DateCreated)",
                    new {Id = application.Data.ApplicationId, Name = application.Data.Name, Status = application.EventType, DateCreated = DateTime.UtcNow});
            }
        }

        public void UpdateApplication(ApplicationContent application)
        {
            using (var conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\ApplicationAdmin.mdf;Integrated Security=True"))
            {
                conn.Open();
                conn.Execute(
                    "update ApplicationStatus set Status = @Status, DateModified = @DateModified  where Id = @Id",
                    new { Id = application.Data.ApplicationId, Status = application.EventType, DateModified = DateTime.UtcNow });
            }
        }
    }
}
