using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.DAO
{
    public interface IProjectsDAO : ICRUDDao<Projects, int>
    {
        bool AddNewProject(string name, int ownerID);

        IEnumerable<Projects> FindAllForAdmin();

        IEnumerable<Projects> FindAllForManager(int managerID);

        bool DeleteById(int projectID);

        bool UpdateStatus(int projectID, bool status);
    }
}
