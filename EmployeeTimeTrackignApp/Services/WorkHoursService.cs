using EmployeeTimeTrackignApp.DAO;
using EmployeeTimeTrackignApp.DAO.Implementation;
using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Services
{
    public class WorkHoursService
    {
        private static IWorkHoursDAO workHoursDAO = new WorkHoursDAO();

        public bool SaveNew(int employeeID, int projectID, int addedHours, string comment)
        {
            return workHoursDAO.SaveNew(employeeID, projectID, addedHours, comment);
        }

        public int AddedHoursSum(int employeeId)
        {
            return workHoursDAO.AddedHoursSum(employeeId);
        }

        public IEnumerable<WorkHours> FindAllByEmployeeID(int employeeID)
        {
            return workHoursDAO.FindAllByEmployeeID(employeeID);
        }

        public bool DeleteById(int whId)
        {
            return workHoursDAO.DeleteById(whId);
        }

        public IEnumerable<ProjectsWorkingHours> WorkingHoursByProject(int employeeID, int projectID)
        {
            return workHoursDAO.WorkingHoursByProject(employeeID, projectID);
        }

        public IEnumerable<WorkHours> FindAllByManagerID(int managerID, DateTime dateRange)
        {
            return workHoursDAO.FindAllByManagerID(managerID, dateRange);
        }

        public bool AcceptOrRejectWorkHours(int workHoursID, string status, string comment)
        {
            return workHoursDAO.AcceptOrRejectWorkHours(workHoursID, status, comment);
        }
    }
}
