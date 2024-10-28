using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Services;
using EmployeeTimeTrackignApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        public Employee Employee { get; }
        public Action CloseWindowAction { get; set; }

        #region VisibilityChange

        private Visibility _changeVisibilityProjectsAdmin;
        public Visibility ChangeVisibilityProjectsAdmin
        {
            get { return _changeVisibilityProjectsAdmin; }
            set
            {
                _changeVisibilityProjectsAdmin = value;
                OnPropertyChanged(nameof(ChangeVisibilityProjectsAdmin));
            }
        }

        private Visibility _changeVisibilityEmployeesAdmin;
        public Visibility ChangeVisibilityEmployeesAdmin
        {
            get { return _changeVisibilityEmployeesAdmin; }
            set
            {
                _changeVisibilityEmployeesAdmin = value;
                OnPropertyChanged(nameof(ChangeVisibilityEmployeesAdmin));
            }
        }

        private Visibility _changeVisibilityProjectsManager;
        public Visibility ChangeVisibilityProjectsManager
        {
            get { return _changeVisibilityProjectsManager; }
            set
            {
                _changeVisibilityProjectsManager = value;
                OnPropertyChanged(nameof(ChangeVisibilityProjectsManager));
            }
        }

        private Visibility _changeVisibilityHoursManagementManager;
        public Visibility ChangeVisibilityHoursManagementManager
        {
            get { return _changeVisibilityHoursManagementManager; }
            set
            {
                _changeVisibilityHoursManagementManager = value;
                OnPropertyChanged(nameof(ChangeVisibilityHoursManagementManager));
            }
        }

        #endregion

        private object _selectedContent;
        public object SelectedContent
        {
            get { return _selectedContent; }
            set
            {
                _selectedContent = value;
                OnPropertyChanged(nameof(SelectedContent));
            }
        }

        public MyICommand<string> ChangeViewCommand { get; set; }

        public EmployeeViewModel(Employee employee)
        {
            Employee = employee;
            if (Employee.Role == EmployeeRole.Employee)
            {
                ChangeVisibilityProjectsAdmin = Visibility.Hidden;
                ChangeVisibilityEmployeesAdmin = Visibility.Hidden;
                ChangeVisibilityProjectsManager = Visibility.Hidden;
                ChangeVisibilityHoursManagementManager = Visibility.Hidden;
            }
            else if (Employee.Role == EmployeeRole.Admin)
            {
                ChangeVisibilityProjectsManager = Visibility.Hidden;
                ChangeVisibilityHoursManagementManager = Visibility.Hidden;
            }
            else
            {
                ChangeVisibilityProjectsAdmin = Visibility.Hidden;
                ChangeVisibilityEmployeesAdmin = Visibility.Hidden;
            }

            ChangeViewCommand = new MyICommand<string>(ChangeView);
            SelectedContent = new EnterHoursView(Employee);
        }

        private void ChangeView(string viewName)
        {
            if (viewName == "EnterHours" && SelectedContent.GetType() != typeof(EnterHoursView))
            {
                SelectedContent = new EnterHoursView(Employee);
            }
            else if (viewName == "ProjectsAdmin" && SelectedContent.GetType() != typeof(ProjectsAdminView))
            {
                SelectedContent = new ProjectsAdminView(Employee);
            }
            else if (viewName == "EmployeesAdmin" && SelectedContent.GetType() != typeof(EmployeesAdminView))
            {
                SelectedContent = new EmployeesAdminView(Employee);
            }
            //else if (viewName == "ProjectsManager" && SelectedContent.GetType() != typeof(ProjectsManagerView))
            //{
            //    SelectedContent = new ProjectsManagerView();
            //}
            //else if (viewName == "HoursManagement" && SelectedContent.GetType() != typeof(HoursManagementView))
            //{
            //    SelectedContent = new HoursManagementView();
            //}
            //else if (viewName == "Statistics" && SelectedContent.GetType() != typeof(StatisticsView))
            //{
            //    SelectedContent = new StatisticsView();
            //}
            else if (viewName == "Profile" && SelectedContent.GetType() != typeof(ProfileView))
            {
                SelectedContent = new ProfileView(Employee);
            }
            else if (viewName == "LogOut")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                CloseWindowAction?.Invoke();
            }
        }
    }
}
