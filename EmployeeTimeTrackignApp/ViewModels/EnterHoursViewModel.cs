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
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class EnterHoursViewModel : BaseViewModel
    {
        public ObservableCollection<Projects> ProjectsCB
        {
            get; set;
        }

        private ObservableCollection<WorkHours> _workingHoursTracking;

        public ObservableCollection<WorkHours> WorkingHoursTracking
        {
            get { return _workingHoursTracking; }
            set
            {
                _workingHoursTracking = value;
                OnPropertyChanged(nameof(WorkingHoursTracking));
            }
        }
        private Employee Employee { get;  }
        private readonly ProjectsService _projectsService = new ProjectsService();
        private readonly WorkHoursService _workHoursService = new WorkHoursService();
        private readonly EmployeeService _employeeService = new EmployeeService();

        private object _selectedProject;
        public object SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
                if (SelectedProject == ProjectsCB[0])
                {
                    AddingHoursOrDaysContent = "Enter free days:";
                    WorkingHoursComment = "Days off";
                    IsCommentReadOnly = true;
                }
                else
                {
                    AddingHoursOrDaysContent = "Enter working hours:";
                    WorkingHoursComment = "";
                    IsCommentReadOnly = false;
                }
            }
        }

        private string _workingHours;
        public string WorkingHours
        {
            get { return _workingHours; }
            set
            {
                SetProperty(ref _workingHours, value);
                AddWorkingHoursCommand.RaiseCanExecuteChanged();
            }
        }

        private string _wHSum;
        public string WHSum
        {
            get { return _wHSum; }
            set
            {
                SetProperty(ref _wHSum, value);
            }
        }

        private string _wHMissingSum;
        public string WHMissingSum
        {
            get { return _wHMissingSum; }
            set
            {
                _wHMissingSum = value;
                OnPropertyChanged(nameof(WHMissingSum));
            }
        }

        private string _wHSumWeek;
        public string WHSumWeek
        {
            get { return _wHSumWeek; }
            set
            {
                SetProperty(ref _wHSumWeek, value);
            }
        }

        private string _workingHoursComment;
        public string WorkingHoursComment
        {
            get { return _workingHoursComment; }
            set
            {
                SetProperty(ref _workingHoursComment, value);
                AddWorkingHoursCommand.RaiseCanExecuteChanged();
            }
        }

        private string _addingHoursOrDaysContent;
        public string AddingHoursOrDaysContent
        {
            get { return _addingHoursOrDaysContent; }
            set
            {
                SetProperty(ref _addingHoursOrDaysContent, value);
            }
        }

        private bool _isCommentReadOnly;
        public bool IsCommentReadOnly
        {
            get { return _isCommentReadOnly; }
            set
            {
                SetProperty(ref _isCommentReadOnly, value);
            }
        }

        public MyICommand AddWorkingHoursCommand { get; set; }
        public MyICommand DeleteWorkingHoursCommand { get; set; }
        public MyICommand<TextBox> TextChangedCommand { get; set; }

        public EnterHoursViewModel(Employee employee)
        {
            Employee = _employeeService.FindByUserID(employee.EmployeeID);

            //Premjesteno iznad zbog WorkingHoursComment-a, jer ne moze da poziva RaiseCanExecuteChanged() prije nego se instancira komanda
            AddWorkingHoursCommand = new MyICommand(OnAddWorkingHours, CanAddWokingHours);
            TextChangedCommand = new MyICommand<TextBox>(OnTextChanged);
            DeleteWorkingHoursCommand = new MyICommand(OnDeleteWorkingHours);

            this.ProjectsCB = (ObservableCollection<Projects>)_projectsService.FindAll();
            if (ProjectsCB.Count != 0)
            {
                SelectedProject = ProjectsCB[0];
            }
            //Provjeriti
            int sum = _workHoursService.AddedHoursSum(Employee.EmployeeID);
            int sumByWeek = _workHoursService.WorkHoursByWeek(Employee.EmployeeID);
            WHSumWeek = Convert.ToString(sumByWeek);
            WHSum = Convert.ToString(sum);
            WHMissingSum = Convert.ToString((8 * GetWorkingDaysInCurrentMonth()) - sum);
            this.WorkingHoursTracking = (ObservableCollection<WorkHours>)_workHoursService.FindAllByEmployeeID(Employee.EmployeeID);

            //AddWorkingHoursCommand = new MyICommand(OnAddWorkingHours, CanAddWokingHours);
            //TextChangedCommand = new MyICommand<TextBox>(OnTextChanged);
            //DeleteWorkingHoursCommand = new MyICommand(OnDeleteWorkingHours);

            WorkingHours = "";
            //WorkingHoursComment = "";
        }

        public int GetWorkingDaysInCurrentMonth()
        {
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int workingDays = 0;

            for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            return workingDays;
        }

        public void OnDeleteWorkingHours()
        {
            var selectedRows = WorkingHoursTracking.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                bool result;
                if (row.ProjectID != 10000)
                {
                    result = _workHoursService.DeleteById(row.WorkHoursID);
                    if (result)
                    {
                        sum++;
                    }
                }
            }

            MessageBox.Show($"Deleted {sum} rows.\nSome hours may not have been deleted because they are leave hours.");
            int sumMonth = _workHoursService.AddedHoursSum(Employee.EmployeeID);
            int sumByWeek = _workHoursService.WorkHoursByWeek(Employee.EmployeeID);
            WHSumWeek = Convert.ToString(sumByWeek);
            WHSum = Convert.ToString(sumMonth);
            WHMissingSum = Convert.ToString((8 * GetWorkingDaysInCurrentMonth()) - sumMonth);
            this.WorkingHoursTracking = (ObservableCollection<WorkHours>)_workHoursService.FindAllByEmployeeID(Employee.EmployeeID);
        }

        //Mora drugacija implementacija da bi se mogla provjera za brisanje izvrsiti
        //public bool IsCheckedRow()
        //{
        //    bool ret = true;

        //    if (WorkingHoursTracking.Where(item => item.SelectedCheck).ToList().Count() == 0)
        //    {
        //        ret = false;
        //    }

        //    return ret;
        //}

        private void OnTextChanged(TextBox textBox)
        {
            if (textBox.Name.Equals("WHTextBox"))
            {
                if (Regex.IsMatch(textBox.Text, @"^\d+$"))
                {
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                        textBox.CaretIndex = textBox.Text.Length;
                    }
                }
            }
            if (textBox.Name.Equals("WHCommentTextBox"))
            {
                if (Regex.IsMatch(textBox.Text, @"^[a-zA-Z0-9\s/-]{0,254}$"))
                {
                    return;
                }
                else
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                    textBox.CaretIndex = textBox.Text.Length;
                }
            }
        }

        public void OnAddWorkingHours()
        {
            Projects project = (Projects)SelectedProject;
            try
            {
                bool result = false;
                if (project.Name.Equals("Free days"))
                {
                    if ((Employee.RemainingLeaveDays - int.Parse(WorkingHours)) >= 0)
                    {
                        result = _workHoursService.SaveNew(Employee.EmployeeID, project.ProjectID, int.Parse(WorkingHours), WorkingHoursComment);
                    }
                }
                else
                {
                    result = _workHoursService.SaveNew(Employee.EmployeeID, project.ProjectID, int.Parse(WorkingHours), WorkingHoursComment);
                }
                
                if (result)
                {
                    if (project.Name.Equals("Free days"))
                    {
                        MessageBox.Show("Free days added.");
                    }
                    else
                    {
                        MessageBox.Show("Working hours added.");
                    }
                    WorkingHours = "";
                    WorkingHoursComment = "";
                    SelectedProject = ProjectsCB[0];

                    int sumMonth = _workHoursService.AddedHoursSum(Employee.EmployeeID);
                    int sumByWeek = _workHoursService.WorkHoursByWeek(Employee.EmployeeID);
                    WHSumWeek = Convert.ToString(sumByWeek);
                    WHSum = Convert.ToString(sumMonth);
                    WHMissingSum = Convert.ToString((8 * GetWorkingDaysInCurrentMonth()) - sumMonth);
                    this.WorkingHoursTracking = (ObservableCollection<WorkHours>)_workHoursService.FindAllByEmployeeID(Employee.EmployeeID);
                }
                else
                {
                    if (project.Name.Equals("Free days") && (Employee.RemainingLeaveDays - int.Parse(WorkingHours)) < 0)
                    {
                        MessageBox.Show($"Failed while trying to add free days.\nYou dont have enough free days left. " +
                            $"You have {Employee.RemainingLeaveDays} remaining free days.");
                    }
                    else
                    {
                        MessageBox.Show("Failed while trying to add working hours.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool CanAddWokingHours()
        {
            bool ret = true;

            if (WorkingHours.Trim().Length > 0)
            {
                int wh;
                try
                {
                    wh = int.Parse(WorkingHours);
                }
                catch
                {
                    ret = false;
                }
            }
            else
            {
                ret = false;
            }

            if (WorkingHoursComment.Trim().Length == 0)
            {
                ret = false;
            }

            else if (Regex.IsMatch(WorkingHoursComment, @".{256,}"))
            {
                ret = false;
            }

            return ret;
        }
    }
}
