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

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class HoursManagementViewModel : BaseViewModel
    {
        private static Employee Employee { get; set; }
        private readonly WorkHoursService _workHoursService = new WorkHoursService();
        public ObservableCollection<WorkHours> AllWorkHoursAdded { get; set; }

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

        public MyICommand AcceptWorkHoursCommand { get; set; }
        public MyICommand RejectWorkHoursCommand { get; set; }

        public HoursManagementViewModel(Employee employee)
        {
            Employee = employee;
            this.AllWorkHoursAdded = (ObservableCollection<WorkHours>)_workHoursService.FindAllByManagerID(Employee.EmployeeID);

            AcceptWorkHoursCommand = new MyICommand(OnAcceptWorkHours);
            RejectWorkHoursCommand = new MyICommand(OnRejectWorkHours, CanRejectWorkHours);
        }

        public void OnAcceptWorkHours()
        {

        }

        public void OnRejectWorkHours()
        {

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
