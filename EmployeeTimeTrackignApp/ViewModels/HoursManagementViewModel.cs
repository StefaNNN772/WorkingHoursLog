using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class HoursManagementViewModel : BaseViewModel
    {
        private static Employee Employee { get; set; }
        private readonly WorkHoursService _workHoursService = new WorkHoursService();

        public ObservableCollection<WorkHoursRangeEnum> WorkHoursRange { get; set; }

        private ObservableCollection<WorkHours> _allWorkHoursAdded;
        public ObservableCollection<WorkHours> AllWorkHoursAdded
        {
            get { return _allWorkHoursAdded; }
            set
            {
                SetProperty(ref _allWorkHoursAdded, value);
            }
        }

        private WorkHours _selectedWorkHours;
        public WorkHours SelectedWorkHours
        {
            get { return _selectedWorkHours; }
            set
            {
                SetProperty(ref _selectedWorkHours, value);
            }
        }

        private string _workHoursRejectionComment;
        public string WorkHoursRejectionComment
        {
            get { return _workHoursRejectionComment; }
            set
            {
                SetProperty(ref _workHoursRejectionComment, value);
                RejectWorkHoursCommand.RaiseCanExecuteChanged();
            }
        }

        private object _selectedRange;
        public object SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                _selectedRange = value;
                OnPropertyChanged(nameof(SelectedRange));
                this.AllWorkHoursAdded = (ObservableCollection<WorkHours>)_workHoursService.FindAllByManagerID(Employee.EmployeeID, GetDateRange());
            }
        }

        public MyICommand AcceptWorkHoursCommand { get; set; }
        public MyICommand RejectWorkHoursCommand { get; set; }

        public HoursManagementViewModel(Employee employee)
        {
            Employee = employee;

            this.WorkHoursRange = new ObservableCollection<WorkHoursRangeEnum>(Enum.GetValues(typeof(WorkHoursRangeEnum)).Cast<WorkHoursRangeEnum>());
            SelectedRange = WorkHoursRange[0];

            //this.AllWorkHoursAdded = (ObservableCollection<WorkHours>)_workHoursService.FindAllByManagerID(Employee.EmployeeID, GetDateRange());

            AcceptWorkHoursCommand = new MyICommand(OnAcceptWorkHours);
            RejectWorkHoursCommand = new MyICommand(OnRejectWorkHours, CanRejectWorkHours);

            WorkHoursRejectionComment = "";
        }

        public DateTime GetDateRange()
        {
            DateTime dateToday = DateTime.Today;
            DateTime result;
            if (SelectedRange.ToString().Equals("Weekly"))
            {
                int diff = (7 + (dateToday.DayOfWeek - DayOfWeek.Monday)) % 7;
                result = dateToday.AddDays(-1 * diff).Date;
            }
            else if (SelectedRange.ToString().Equals("Monthly"))
            {
                result = new DateTime(dateToday.Year, dateToday.Month, 1);
            }
            else if (SelectedRange.ToString().Equals("Yearly"))
            {
                result = new DateTime(dateToday.Year, 1, 1);
            }
            else
            {
                result = new DateTime(1753, 1, 1);
            }

            return result;
        }

        public void OnAcceptWorkHours()
        {
            var selectedRows = AllWorkHoursAdded.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                if (!row.Status.Equals("Accepted"))
                {
                    bool result = _workHoursService.AcceptOrRejectWorkHours(row.WorkHoursID, "Accepted", row.Comment);
                    if (result)
                    {
                        sum++;
                    }
                }
            }

            MessageBox.Show($"You have updated status of {sum} added working hours.\nSome added working hours may have been already updated.");
            WorkHoursRejectionComment = "";
            this.AllWorkHoursAdded = (ObservableCollection<WorkHours>)_workHoursService.FindAllByManagerID(Employee.EmployeeID, GetDateRange());
        }

        public void OnRejectWorkHours()
        {
            var selectedRows = AllWorkHoursAdded.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                if (!row.Status.Equals("Rejected"))
                {
                    bool result = _workHoursService.AcceptOrRejectWorkHours(row.WorkHoursID, "Rejected", WorkHoursRejectionComment);
                    if (result)
                    {
                        sum++;
                    }
                }
            }

            MessageBox.Show($"You have updated status of {sum} added working hours.\nSome added working hours may have been already updated.");
            WorkHoursRejectionComment = "";
            this.AllWorkHoursAdded = (ObservableCollection<WorkHours>)_workHoursService.FindAllByManagerID(Employee.EmployeeID, GetDateRange());
        }

        public bool CanRejectWorkHours()
        {
            bool ret = true;

            if (String.IsNullOrEmpty(WorkHoursRejectionComment))
            {
                ret = false;
            }
            else if(Regex.IsMatch(WorkHoursRejectionComment, @".{256,}"))
            {
                ret = false;
            }
            else if (WorkHoursRejectionComment.Trim().Length == 0)
            {
                ret = false;
            }

            return ret;
        }
    }
}
