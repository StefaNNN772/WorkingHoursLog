using EmployeeTimeTrackignApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTrackignApp.Models
{
    public class Employee : BaseViewModel
    {
        //[RegularExpression("")]
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime PasswordCreatedAt { get; set; }
        public string Email { get; set; }
        public EmployeeRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RemainingLeaveDays { get; set; }
        public bool PasswordUpdated { get; set; }

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
    }
}
