using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AdminWeb.Models;
using Dapper;

namespace AdminWeb.Repositories
{
    public interface IApplicationRepository
    {
        IEnumerable<ApplicationViewModel> GetApplications();
    }

    public class ApplicationRepository : IApplicationRepository
    {
        private const string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\EventSourcing\Projection\Database\ApplicationAdmin.mdf;Integrated Security=True";

        public IEnumerable<ApplicationViewModel> GetApplications()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    var applications = conn.Query<ApplicationViewModel>("SELECT * FROM Apply");

                    return applications;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}