using EmployeeTimeTrackignApp.Connections;
using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeTimeTrackignApp.DAO.Implementation
{
    public class WorkHoursDAO : IWorkHoursDAO
    {
        public int AddedHoursSum(int employeeId)
        {
            int ret = 0;
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("SELECT Sum(AddedHours) " +
                    "FROM WorkHours WHERE CreatedAt >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0) " +
                    "AND CreatedAt <= GETDATE() " +
                    "AND Status = 'Accepted' " +
                    "AND EmployeeID = @EmployeeID", conn);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                object result = command.ExecuteScalar();

                conn.Close();

                if (result != null && result != DBNull.Value)
                {
                    ret = Convert.ToInt32(result);
                }
            }

            return ret;
        }

        public bool DeleteById(int whId)
        {
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand("DELETE FROM WorkHours WHERE WorkHoursID = @WorkHoursID", conn);
                    command.Parameters.AddWithValue("@WorkHoursID", whId);

                    command.Transaction = transaction;

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Error: {ex.Message}");
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool ExistsById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkHours> FindAll()
        {
            throw new NotImplementedException();
        }

        public WorkHours FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkHours> FintAllByEmployeeID(int employeeID)
        {
            ObservableCollection<WorkHours> ret = new ObservableCollection<WorkHours>();
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("SELECT * FROM WorkHours " +
                    "WHERE EmployeeID = @EmployeeID", conn);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new WorkHours(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3),
                        reader.GetDateTime(4), reader.GetString(5), reader.GetString(6)));
                }

                conn.Close();
            }

            return ret;
        }

        public string Save(WorkHours entity)
        {
            throw new NotImplementedException();
        }

        public bool SaveNew(int employeeID, int projectID, int addedHours, string comment)
        {
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("INSERT INTO WorkHours (EmployeeID, ProjectID, AddedHours, Comment) " +
                    "VALUES (@EmployeeID, @ProjectID, @AddedHours, @Comment)", conn);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@ProjectID", projectID);
                command.Parameters.AddWithValue("@AddedHours", addedHours);
                command.Parameters.AddWithValue("@Comment", comment);

                int rowAffected = command.ExecuteNonQuery();

                conn.Close();

                return rowAffected > 0;
            }
        }
    }
}
