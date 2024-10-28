using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.DAO
{
    public interface IEmployeeDAO : ICRUDDao<Employee, int>
    {
        bool Update(Employee employee);

        Employee FindByUsername(string username, string password);

        bool ExistsByUsername(string username);

        bool AddNewEmployee(string username, string password, string email, string role, int remainingLeaveDays);

        IEnumerable<Employee> FindAllEmployees(int adminId);

        IEnumerable<Employee> FindAllManagers();

        bool DeleteById(int employeeID);

        void CheckPasswordChanged(DateTime time);

        bool CreateNewPassword(int employeeID, string password);
    }
}
