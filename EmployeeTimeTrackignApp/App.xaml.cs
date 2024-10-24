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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _passwordCheckService = new PasswordCheckService();

            _passwordCheckService.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _passwordCheckService?.Stop();
            base.OnExit(e);
        }
    }

}
