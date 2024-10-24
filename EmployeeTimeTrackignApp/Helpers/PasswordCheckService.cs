using EmployeeTimeTrackignApp.Connections;
using EmployeeTimeTrackignApp.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Helpers
{
    public class PasswordCheckService
    {
        private Timer _timer;
        private readonly EmployeeService _employeeService = new EmployeeService();

        public void Start()
        {
            _timer = new Timer(PasswordCheck, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        }

        public void PasswordCheck(object state)
        {
            DateTime now = DateTime.Now;

            DateTime before48h = now.AddHours(-48);

            _employeeService.CheckPasswordChanged(before48h);
        }

        public void Stop()
        {
            _timer?.Dispose();
        }
    }
}
