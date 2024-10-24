using EmployeeTimeTrackignApp.Connections;
using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.DAO.Implementation
{
    public class ProjectsDAO : IProjectsDAO
    {
        public bool ExistsById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Projects> FindAll()
        {
            ObservableCollection<Projects> projects = new ObservableCollection<Projects>();
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Projects WHERE IsActive = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    projects.Add(new Projects
                    {
                        ProjectID = reader.GetInt32(0),
                        OwnerID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        IsActive = reader.GetBoolean(3),
                        CreatedAt = reader.GetDateTime(4)
                    });
                }

                conn.Close();
            }
            return projects;
        }

        public Projects FindById(int id)
        {
            throw new NotImplementedException();
        }

        public string Save(Projects entity)
        {
            throw new NotImplementedException();
        }
    }
}
