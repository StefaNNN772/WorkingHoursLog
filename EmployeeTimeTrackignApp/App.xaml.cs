using EmployeeTimeTrackignApp.Helpers;
using System.Configuration;
using System.Data;
using System.Windows;

namespace EmployeeTimeTrackignApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private PasswordCheckService _passwordCheckService;
        private WorkHoursCheckService _workHoursCheckService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _passwordCheckService = new PasswordCheckService();

            _workHoursCheckService = new WorkHoursCheckService();

            _passwordCheckService.Start();

            _workHoursCheckService.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _passwordCheckService?.Stop();
            _workHoursCheckService?.Stop();
            base.OnExit(e);
        }
    }

}
