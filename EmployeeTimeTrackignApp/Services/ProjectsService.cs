using EmployeeTimeTrackignApp.DAO.Implementation;
using EmployeeTimeTrackignApp.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeTimeTrackignApp.Models;

namespace EmployeeTimeTrackignApp.Services
{
    public class ProjectsService
    {
        private static IProjectsDAO projectsDAO = new ProjectsDAO();

        public IEnumerable<Projects> FindAll()
        {
            return projectsDAO.FindAll();
        }
    }
}
