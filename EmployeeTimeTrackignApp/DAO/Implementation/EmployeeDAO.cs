using EmployeeTimeTrackignApp.Connections;
using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeTimeTrackignApp.DAO.Implementation
{
    public class EmployeeDAO : IEmployeeDAO
    {
        public bool AddNewEmployee(string username, string password, string email, string role, int remainingLeaveDays)
        {
            HashPassword hash = new HashPassword();
            string newHashPassword = hash.Hash(password);
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("INSERT INTO Employees (Username, Password, Email, Role, RemainingLeaveDays) " +
                    "VALUES(@Username, @Password, @Email, @Role, @RemainingLeaveDays)", conn);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", newHashPassword);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@RemainingLeaveDays", remainingLeaveDays);

                int result = command.ExecuteNonQuery();

                conn.Close();

                return result > 0;
            }
        }

        public void CheckPasswordChanged(DateTime time)
        {
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand("UPDATE Employees SET IsActive = 0 WHERE PasswordUpdated = 0 AND " +
                    "PasswordCreatedAt < @Before48h AND IsActive = 1", conn);

                    command.Parameters.AddWithValue("@Before48h", time);

                    command.Transaction = transaction;

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    transaction.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool CreateNewPassword(int employeeID, string password)
        {
            HashPassword hash = new HashPassword();
            string newHashPassword = hash.Hash(password);
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand("UPDATE Employees SET Password = @Password, IsActive = 1 WHERE EmployeeID = @EmployeeID", conn);

                    command.Parameters.AddWithValue("@Password", newHashPassword);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

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
                    MessageBox.Show($"Error: {ex.Message}");
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DeleteById(int employeeID)
        {
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID " +
                        "AND EmployeeID NOT IN (SELECT OwnerID FROM Projects)", conn);

                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

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

        public bool ExistsByUsername(string username)
        {
            int count = 0;
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM Employees WHERE Username = @Username", conn);
                command.Parameters.AddWithValue("@Username", username);

                count = (int)command.ExecuteScalar();

                conn.Close();
            }

            return count > 0;
        }

        public IEnumerable<Employee> FindAll()
        {
            ObservableCollection<Employee> ret = new ObservableCollection<Employee>();
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("SELECT EmployeeID, Username, Email, Role, IsActive, RemainingLeaveDays, PasswordUpdated " +
                    "FROM Employees", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(3);
                    EmployeeRole roleNew;
                    if (role == "Employee")
                    {
                        roleNew = EmployeeRole.Employee;
                    }
                    else if (role == "Admin")
                    {
                        roleNew = EmployeeRole.Admin;
                    }
                    else
                    {
                        roleNew = EmployeeRole.Manager;
                    }
                    ret.Add(new Employee
                    {
                        EmployeeID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        Role = roleNew,
                        IsActive = reader.GetBoolean(4),
                        RemainingLeaveDays = reader.GetInt32(5),
                        PasswordUpdated = reader.GetBoolean(6)
                    });
                }

                conn.Close();
            }

            return ret;
        }

        public IEnumerable<Employee> FindAllEmployees(int adminId)
        {
            ObservableCollection<Employee> ret = new ObservableCollection<Employee>();
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("FindAllEmployees", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AdminID", adminId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(3);
                    EmployeeRole roleNew;
                    if (role == "Employee")
                    {
                        roleNew = EmployeeRole.Employee;
                    }
                    else if (role == "Admin")
                    {
                        roleNew = EmployeeRole.Admin;
                    }
                    else
                    {
                        roleNew = EmployeeRole.Manager;
                    }
                    ret.Add(new Employee { EmployeeID = reader.GetInt32(0), Username = reader.GetString(1), Email = reader.GetString(2),
                        Role = roleNew, IsActive = reader.GetBoolean(4), RemainingLeaveDays = reader.GetInt32(5),
                        PasswordUpdated = reader.GetBoolean(6) });
                }

                conn.Close();
            }

            return ret;
        }

        public IEnumerable<Employee> FindAllManagers()
        {
            ObservableCollection<Employee> ret = new ObservableCollection<Employee>();
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand command = new SqlCommand("FindAllManagersForAdmin", conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new Employee
                    {
                        EmployeeID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        Role = EmployeeRole.Manager,
                        IsActive = reader.GetBoolean(3),
                        RemainingLeaveDays = reader.GetInt32(4),
                        PasswordUpdated = reader.GetBoolean(5)
                    });
                }

                conn.Close();
            }

            return ret;
        }

        public Employee FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Employee FindByUserID(int employeeID)
        {
            Employee employee = null;
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("GetEmployeeByUserID", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader.GetString(5);
                    EmployeeRole roleNew = role switch
                    {
                        "Employee" => EmployeeRole.Employee,
                        "Admin" => EmployeeRole.Admin,
                        _ => EmployeeRole.Manager
                    };
                    employee = new Employee
                    {
                        EmployeeID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        PasswordCreatedAt = reader.GetDateTime(3),
                        Email = reader.GetString(4),
                        Role = roleNew,
                        IsActive = reader.GetBoolean(6),
                        CreatedAt = reader.GetDateTime(7),
                        RemainingLeaveDays = reader.GetInt32(8),
                        PasswordUpdated = reader.GetBoolean(9)
                    };
                }

                conn.Close();
            }
            return employee;
        }

        public Employee FindByUsername(string username, string password)
        {
            Employee employee = null;
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("GetEmployeeByCredentials", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader.GetString(5);
                    EmployeeRole roleNew = role switch
                    {
                        "Employee" => EmployeeRole.Employee,
                        "Admin" => EmployeeRole.Admin,
                        _ => EmployeeRole.Manager
                    };
                    employee = new Employee
                    {
                        EmployeeID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        PasswordCreatedAt = reader.GetDateTime(3),
                        Email = reader.GetString(4),
                        Role = roleNew,
                        IsActive = reader.GetBoolean(6),
                        CreatedAt = reader.GetDateTime(7),
                        RemainingLeaveDays = reader.GetInt32(8),
                        PasswordUpdated = reader.GetBoolean(9)
                    };
                }

                conn.Close();
            }
            return employee;
        }

        public string Save(Employee entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Employee employee)
        {
            HashPassword hash = new HashPassword();
            string password = hash.Hash(employee.Password);
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand("UPDATE Employees SET Username = @Username, Email = @Email, Password = @Password, " +
                        "IsActive = @IsActive, PasswordUpdated = 1 WHERE EmployeeID = @EmployeeID", conn);
                    command.Parameters.AddWithValue("@Username", employee.Username);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@IsActive", employee.IsActive);
                    command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);

                    command.Transaction = transaction;

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
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
                    MessageBox.Show($"Error: {ex.Message}");
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
