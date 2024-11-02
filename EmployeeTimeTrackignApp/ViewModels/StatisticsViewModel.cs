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
using System.Windows.Media;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private static Employee Employee { get; set; }
        public ObservableCollection<Projects> ProjectsCB { get; set; }
        public ObservableCollection<ProjectsWorkingHours> ProjectsWH { get; set; }
        private readonly ProjectsService _projectsService = new ProjectsService();
        private readonly WorkHoursService _workHoursService = new WorkHoursService();

        public SeriesCollection ProjectHoursSeries { get; set; }

        private Projects _selectedProject;
        public Projects SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                SetProperty(ref _selectedProject, value);
                ShowStatistics();
            }
        }

        public StatisticsViewModel(Employee employee)
        {
            Employee = employee;

            ProjectHoursSeries = new SeriesCollection();

            this.ProjectsCB = (ObservableCollection<Projects>)_projectsService.FindAllForStatistics(Employee.EmployeeID);
            if (ProjectsCB.Count != 0)
            {
                SelectedProject = ProjectsCB[0];
            }
        }

        public void ShowStatistics()
        {
            if (SelectedProject != null)
            {
                this.ProjectsWH = (ObservableCollection<ProjectsWorkingHours>)_workHoursService.WorkingHoursByProject(Employee.EmployeeID, SelectedProject.ProjectID);

                ProjectHoursSeries.Clear();

                foreach (var projectWH in ProjectsWH)
                {
                    ProjectHoursSeries.Add(new PieSeries
                    {
                        Title = projectWH.Status,
                        Values = new ChartValues<double> { projectWH.WorkingHours },
                        DataLabels = true,
                        Fill = GetColorByStatus(projectWH.Status)
                    });
                }
            }
        }

        public Brush GetColorByStatus(string status)
        {
            switch (status)
            {
                case "Accepted":
                    return Brushes.DeepSkyBlue;
                case "Pending":
                    return Brushes.Yellow;
                case "Rejected":
                    return Brushes.Red;
                default:
                    return Brushes.Gray;
            }
        }
    }
}
