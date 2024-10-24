using EmployeeTimeTrackignApp.DAO;
using EmployeeTimeTrackignApp.DAO.Implementation;
using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Services
{
    public class EmployeeService
    {
        private static IEmployeeDAO employeeDAO = new EmployeeDAO();

        public bool Update(Employee employee)
        {
            return employeeDAO.Update(employee);
        }

        public Employee FindByUsername(string username, string password)
        {
            return employeeDAO.FindByUsername(username, password);
        }

        public bool ExistsByUsername(string username)
        {
            return employeeDAO.ExistsByUsername(username);
        }

        public bool AddNewEmployee(string username, string password, string email, string role, int remainingLeaveDays)
        {
            return employeeDAO.AddNewEmployee(username, password, email, role, remainingLeaveDays);
        }

        public IEnumerable<Employee> FindAllEmployees(int adminId)
        {
            return employeeDAO.FindAllEmployees(adminId);
        }

        public bool DeleteById(int employeeID)
        {
            return employeeDAO.DeleteById(employeeID);
        }

        public void CheckPasswordChanged(DateTime time)
        {
            employeeDAO.CheckPasswordChanged(time);
        }

        public bool CreateNewPassword(int employeeID, string password)
        {
            return employeeDAO.CreateNewPassword(employeeID, password);
        }
    }
}
