using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class ProjectsAdminViewModel : BaseViewModel
    {
        private static Employee Employee { get; set; }
        private readonly EmployeeService _employeeService = new EmployeeService();
        private readonly ProjectsService _projectsService = new ProjectsService();
        private static EmailService _emailService = new EmailService();

        private ObservableCollection<Employee> _allManagers;
        public ObservableCollection<Employee> AllManagers
        {
            get { return _allManagers; }
            set
            {
                SetProperty(ref _allManagers, value);
            }
        }

        private Employee _selectedManager;
        public Employee SelectedManager
        {
            get { return _selectedManager; }
            set
            {
                SetProperty(ref _selectedManager, value);
            }
        }

        private ObservableCollection<Projects> _allProjects;
        public ObservableCollection<Projects> AllProjects
        {
            get { return _allProjects; }
            set
            {
                SetProperty(ref _allProjects, value);
            }
        }

        private Projects _selectedProject;
        public Projects SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                SetProperty(ref _selectedProject, value);
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
                AddProjectCommand.RaiseCanExecuteChanged();
            }
        }

        public MyICommand AddProjectCommand { get; set; }
        public MyICommand DeleteProjectsCommand { get; set; }

        public ProjectsAdminViewModel(Employee employee)
        {
            Employee = employee;
            this.AllProjects = (ObservableCollection<Projects>)_projectsService.FindAllForAdmin();
            this.AllManagers = (ObservableCollection<Employee>)_employeeService.FindAllManagers();
            SelectedManager = AllManagers[0];

            AddProjectCommand = new MyICommand(OnAddProject, CanAddProject);
            DeleteProjectsCommand = new MyICommand(OnDeleteProject);
        }

        public void OnDeleteProject()
        {
            var selectedRows = AllProjects.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                bool result = _projectsService.DeleteById(row.ProjectID);

                if (result)
                {
                    sum++;
                }
            }

            MessageBox.Show($"Deleted {sum} project(s).\nSome projects may not have been deleted because they are active and have recorded working hours.");
            this.AllProjects = (ObservableCollection<Projects>)_projectsService.FindAllForAdmin();
        }

        public void OnAddProject()
        {
            bool result = _projectsService.AddNewProject(ProjectName, SelectedManager.EmployeeID);
            if (result)
            {
                MessageBox.Show("New project added.");

                string name = ProjectName;

                Thread emailThread = new Thread(() =>
                {
                    _emailService.SendEmail(SelectedManager.Email, "New project created", $"You are added as owner of new project: {name}. Please update status of the project.");
                });
                emailThread.IsBackground = true;
                emailThread.Start();

                ProjectName = "";
                SelectedManager = AllManagers[0];
                this.AllProjects = (ObservableCollection<Projects>)_projectsService.FindAllForAdmin();
            }
        }

        public bool CanAddProject()
        {
            bool ret = true;

            if (String.IsNullOrEmpty(ProjectName))
            {
                ret = false;
            }

            return ret;
        }
    }
}
