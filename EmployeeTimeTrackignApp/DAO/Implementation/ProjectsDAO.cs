﻿using EmployeeTimeTrackignApp.Connections;
using EmployeeTimeTrackignApp.Helpers;
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
    public class ProjectsDAO : IProjectsDAO
    {
        public bool AddNewProject(string name, int ownerID)
        {
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Projects (OwnerID, Name) " +
                        "VALUES (@OwnerID, @Name)", conn);

                    command.Parameters.AddWithValue("@OwnerID", ownerID);
                    command.Parameters.AddWithValue("@Name", name);

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

        public bool DeleteById(int projectID)
        {
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand("DELETE FROM Projects WHERE ProjectID = @ProjectID AND (IsActive = 0 OR ProjectID NOT IN (" +
                        "SELECT ProjectID FROM WorkHours))", conn);

                    command.Parameters.AddWithValue("@ProjectID", projectID);

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

        public IEnumerable<Projects> FindAllForAdmin()
        {
            ObservableCollection<Projects> projects = new ObservableCollection<Projects>();
            using (SqlConnection conn = ConnectionUtil_Pooling.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT ProjectID, OwnerID, Name, IsActive, CreatedAt FROM Projects", conn);
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
