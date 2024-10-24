using EmployeeTimeTrackignApp.Connections;
using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Views;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly EmployeeService _employeeService = new EmployeeService();
        private readonly HashPassword _hashPassword = new HashPassword();
        private readonly Window _currentWindow;

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public MyICommand<object>LoginCommand { get; }

        public MainWindowViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;
            LoginCommand = new MyICommand<object>(Login);
        }

        private void Login(object parameter)
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            string username = Username;
            string password = _hashPassword.Hash(passwordBox.Password);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                Employee employee = _employeeService.FindByUsername(username, password);
                if (employee == null)
                {
                    MessageBox.Show("Invalid username or password.");
                }
                else
                {
                    if (!employee.IsActive)
                    {
                        MessageBox.Show("Your account is inactive.");
                        return;
                    }
                    var employeeViewModel = new EmployeeViewModel(employee);
                    EmployeeView emp = new EmployeeView(employeeViewModel);
                    _currentWindow.Close();
                    emp.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
