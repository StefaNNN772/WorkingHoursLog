using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class ProjectsManagerViewModel : BaseViewModel
    {
        private static Employee Employee { get; set; }
        private readonly ProjectsService _projectsService = new ProjectsService();

        private ObservableCollection<Projects> _allProjects;
        public ObservableCollection<Projects> AllProjects
        {
            get { return _allProjects; }
            set
            {
                SetProperty(ref _allProjects, value);
            }
        }

        public SeriesCollection ProjectHoursSeries { get; set; }
        private ObservableCollection<ProjectsWorkingHours> _projectsWH;
        public ObservableCollection<ProjectsWorkingHours> ProjectsWH
        {
            get { return _projectsWH; }
            set
            {
                SetProperty(ref _projectsWH, value);
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

        public MyICommand ActivateProjectCommand { get; set; }
        public MyICommand DeactivateProjectCommand { get; set; }

        public ProjectsManagerViewModel(Employee employee)
        {
            Employee = employee;
            this.AllProjects = (ObservableCollection<Projects>)_projectsService.FindAllForManager(Employee.EmployeeID);

            this.ProjectsWH = (ObservableCollection<ProjectsWorkingHours>)_projectsService.WorkingHoursByProject(Employee.EmployeeID);
            ProjectHoursSeries = new SeriesCollection();
            foreach (var projectWH in ProjectsWH)
            {
                ProjectHoursSeries.Add(new PieSeries
                {
                    Title = projectWH.Name,
                    Values = new ChartValues<double> { projectWH.WorkingHours },
                    DataLabels = true
                });
            }

            ActivateProjectCommand = new MyICommand(OnActivateProject);
            DeactivateProjectCommand = new MyICommand(OnDeactivateProject);
        }

        public void OnActivateProject()
        {
            var selectedRows = AllProjects.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                if (!row.IsActive)
                {
                    bool result = _projectsService.UpdateStatus(row.ProjectID, true);
                    if (result)
                    {
                        sum++;
                    }
                }
            }

            MessageBox.Show($"You have updated status of {sum} project(s).\nSome projects may have been already activated");
            this.AllProjects = (ObservableCollection<Projects>)_projectsService.FindAllForManager(Employee.EmployeeID);
        }

        //public bool CanActivateProject()
        //{
        //    bool ret = true;

        //    var selectedRows = AllProjects.Where(item => item.SelectedCheck).ToList();

        //    if (selectedRows.Count == 0)
        //    {
        //        ret = false;
        //    }

        //    return ret;
        //}

        public void OnDeactivateProject()
        {
            var selectedRows = AllProjects.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                if (row.IsActive)
                {
                    bool result = _projectsService.UpdateStatus(row.ProjectID, false);
                    if (result)
                    {
                        sum++;
                    }
                }
            }

            MessageBox.Show($"You have updated status of {sum} project(s).\nSome projects may have been already deactivated");
            this.AllProjects = (ObservableCollection<Projects>)_projectsService.FindAllForManager(Employee.EmployeeID);
        }
    }
}
