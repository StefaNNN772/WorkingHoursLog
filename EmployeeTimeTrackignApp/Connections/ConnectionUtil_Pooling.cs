using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Connections
{
    public class ConnectionUtil_Pooling : IDisposable
    {
        private static SqlConnection instance = null;

        public static SqlConnection GetConnection()
        {
            try
            {
                instance = new SqlConnection(ConnectionParams.GetConnectionString());

                instance.Open();

                return instance;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not open a connection to the database.", ex);
            }
        }

        public void Dispose()
        {
            if (instance != null)
            {
                instance.Close();
                instance.Dispose();
            }
        }
    }
}
