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

        public bool AddNewProject(string name, int ownerID)
        {
            return projectsDAO.AddNewProject(name, ownerID);
        }

        public IEnumerable<Projects> FindAllForAdmin()
        {
            return projectsDAO.FindAllForAdmin();
        }

        public bool DeleteById(int projectID)
        {
            return projectsDAO.DeleteById(projectID);
        }
    }
}
