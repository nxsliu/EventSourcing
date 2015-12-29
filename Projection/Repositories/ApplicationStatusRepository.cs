using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Models;

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
            throw new NotImplementedException();
        }

        public void CreateApplication(ApplicationContent application)
        {
            throw new NotImplementedException();
        }

        public void UpdateApplication(ApplicationContent application)
        {
            throw new NotImplementedException();
        }
    }
}
