using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Helpers
{
    public class WorkHoursCheckService
    {
        private Timer _timer;
        private readonly EmployeeService _employeeService = new EmployeeService();
        private readonly WorkHoursService _workHoursService = new WorkHoursService();
        private static EmailService _emailService = new EmailService();
        private ObservableCollection<Employee> Employees { get; set; }

        public void Start()
        {
            _timer = new Timer(WorkHoursCheck, null, TimeSpan.FromSeconds(20), TimeSpan.FromDays(1));
        }

        private void WorkHoursCheck(object state)
        {
            _ = WorkHoursCheckAsync(state);
        }

        public async Task WorkHoursCheckAsync(object state)
        {
            DateTime todayDate = DateTime.Today;
            DateTime endOfMonth = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            int daysLeft = (endOfMonth - todayDate).Days;

            if (daysLeft == 2)
            {
                Employees = (ObservableCollection<Employee>)_employeeService.FindAll();
                
                
                List<Task> emailTasks = new List<Task>();

                foreach (Employee employee in Employees)
                {
                    int workingHours = _workHoursService.WorkHoursCheck(employee.EmployeeID);

                    if (workingHours < GetWorkingDaysInCurrentMonth())
                    {
                        emailTasks.Add(Task.Run(() =>
                        {
                            _emailService.SendEmail(employee.Email, "Working hours", $"Hello {employee.Username}. Please enter working hours before the end of the month.");
                        }));
                    }
                }

                await Task.WhenAll(emailTasks);
            }
        }

        public int GetWorkingDaysInCurrentMonth()
        {
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int workingDays = 0;

            for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            return workingDays;
        }

        public void Stop()
        {
            _timer?.Dispose();
        }
    }
}
