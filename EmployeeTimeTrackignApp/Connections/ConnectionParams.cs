using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Connections
{
    public class ConnectionParams
    {
        public static readonly string ServerName = "STEFAN\\SQLEXPRESS";
        public static readonly string DatabaseName = "EmployeesWorkHoursLog";
        public static readonly string IntegratedSecurity = "True";

        public static string GetConnectionString()
        {
            return $"Server={ServerName};Database={DatabaseName};Integrated Security={IntegratedSecurity};Pooling=True;Min Pool Size=1;Max Pool Size=10;Connection Lifetime=5;Connection Timeout=30;";
        }
    }
}
