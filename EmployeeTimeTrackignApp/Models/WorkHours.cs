using EmployeeTimeTrackignApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Models
{
    public class WorkHours : BaseViewModel
    {
        public int WorkHoursID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public int AddedHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }

        //Dodato zbog lakseg selektovanja u DataGridu
        private bool _selectedCheck;
        public bool SelectedCheck
        {
            get { return _selectedCheck; }
            set
            {
                SetProperty(ref _selectedCheck, value);
            }
        }

        public WorkHours(int workHoursId, int employeeId, int projectId, int addedHours, DateTime createdAt, string status, string comment)
        {
            this.WorkHoursID = workHoursId;
            this.EmployeeID = employeeId;
            this.ProjectID = projectId;
            this.AddedHours = addedHours;
            this.CreatedAt = createdAt;
            this.Status = status;
            this.Comment = comment;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
