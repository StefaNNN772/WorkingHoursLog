﻿using System;
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

        //Dodato zbog lakseg selektovanja u DataGridu
        private bool _selectedCheck;
        public bool SelectedCheck
        {
            get { return _selectedCheck; }
            set
            {
                _selectedCheck = value;
                OnPropertyChanged(nameof(_selectedCheck));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
