using EmployeeTimeTrackignApp.Helpers;
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
        IEnumerable<WorkHours> FindAllByEmployeeID(int employeeID);
        int AddedHoursSum(int employeeId);

        bool DeleteById(int whId);

        IEnumerable<ProjectsWorkingHours> WorkingHoursByProject(int employeeID, int projectID);

        IEnumerable<WorkHours> FindAllByManagerID(int managerID, DateTime dateRange);

        bool AcceptOrRejectWorkHours(int workHoursID, string status, string comment);

        int WorkHoursCheck(int employeeID);

        int WorkHoursByWeekCheck(int employeeID);

        int WorkHoursByWeek(int employeeID);
    }
}
