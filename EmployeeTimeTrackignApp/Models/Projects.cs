using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Models
{
    public class Projects : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int projectID;
        public int OwnerID { get; set; }
        string name;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        //public Projects(int projectId, int ownerId, string name, bool isActive, DateTime createdAt)
        //{
        //    this.ProjectID = projectId;
        //    this.OwnerID = ownerId;
        //    this.Name = name;
        //    this.IsActive = isActive;
        //    this.CreatedAt = createdAt;
        //}

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int ProjectID
        {
            get
            {
                return projectID;
            }
            set
            {
                if (projectID != value)
                {
                    projectID = value;
                    OnPropertyChanged(nameof(projectID));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
