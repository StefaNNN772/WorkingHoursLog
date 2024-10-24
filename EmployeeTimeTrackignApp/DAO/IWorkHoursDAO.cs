using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.DAO
{
    public interface IWorkHoursDAO : ICRUDDao<WorkHours, int>
    {
        bool SaveNew(int employeeID, int projectID, int addedHours, string comment);
        public IEnumerable<WorkHours> FintAllByEmployeeID(int employeeID);
        public int AddedHoursSum(int employeeId);

        public bool DeleteById(int whId);
    }
}
