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

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class EmployeesAdminViewModel : BaseViewModel
    {
        private static Employee Employee { get; set; }
        private readonly EmployeeService _employeeService = new EmployeeService();
        private static EmailService _emailService = new EmailService();
        public ObservableCollection<string> EmployeesRole { get; set; }

        private ObservableCollection<Employee> _allEmployees;
        public ObservableCollection<Employee> AllEmployees
        {
            get { return _allEmployees; }
            set
            {
                SetProperty(ref _allEmployees, value);
            }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value);
                UpdateEmployeeCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isTextBoxReadOnly;
        public bool IsTextBoxReadOnly
        {
            get { return _isTextBoxReadOnly; }
            set
            {
                SetProperty(ref _isTextBoxReadOnly, value);
            }
        }

        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set
            {
                SetProperty(ref _isButtonEnable, value);
            }
        }

        private bool _isComboBoxEnable;
        public bool IsComboBoxEnable
        {
            get { return _isComboBoxEnable; }
            set
            {
                SetProperty(ref _isComboBoxEnable, value);
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
            }
        }

        private string _remainingLeaveDays;
        public string RemainingLeaveDays
        {
            get { return _remainingLeaveDays; }
            set
            {
                SetProperty(ref _remainingLeaveDays, value);
            }
        }

        private object _selectedRole;
        public object SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public MyICommand<object> AddNewEmployeeCommand { get; set; }
        public MyICommand DeleteEmployeesCommand { get; set; }
        public MyICommand<object> UpdateEmployeeCommand { get; set; }

        public EmployeesAdminViewModel(Employee employee)
        {
            Employee = employee;
            //EmployeesRole = new ObservableCollection<EmployeeRole>(Enum.GetValues(typeof(EmployeeRole)).Cast<EmployeeRole>());
            EmployeesRole = new ObservableCollection<string> { "Employee", "Manager" };
            SelectedRole = EmployeesRole[0];
            IsButtonEnable = true;
            IsTextBoxReadOnly = false;
            IsComboBoxEnable = true;

            this.AllEmployees = (ObservableCollection<Employee>)_employeeService.FindAllEmployees(Employee.EmployeeID);

            AddNewEmployeeCommand = new MyICommand<object>(OnAddNewEmployee);
            DeleteEmployeesCommand = new MyICommand(OnDeleteEmployees);
            UpdateEmployeeCommand = new MyICommand<object>(OnUpdateEmployee, CanUpdateEmployee);
        }

        public void OnUpdateEmployee(object parameter)
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            if (!Regex.IsMatch(passwordBox.Password, @"[a-zA-Z\d_]+"))
            {
                MessageBox.Show("For password you can use: a-z, A-z, 0-9 and _");
                return;
            }

            bool result = _employeeService.CreateNewPassword(SelectedEmployee.EmployeeID, passwordBox.Password);
            if (result)
            {
                bool emailSent = _emailService.SendEmail(SelectedEmployee.Email, "Password updated", $"Your username is: " +
                    $"{SelectedEmployee.Username} and your password is: {passwordBox.Password} . Please update your password in next 48h.");
                MessageBox.Show("Employee password udapted.");
                if (emailSent)
                {
                    MessageBox.Show("Email is sent to the adderess of new employee.");
                }
                else
                {
                    MessageBox.Show("Email isnt sent to the address of new employee.");
                }
                Username = "";
                SelectedRole = EmployeesRole[0];
                RemainingLeaveDays = "";
                Email = "";
                passwordBox.Password = "";
                IsButtonEnable = true;
                IsTextBoxReadOnly = false;
                IsComboBoxEnable = true;
                this.AllEmployees = (ObservableCollection<Employee>)_employeeService.FindAllEmployees(Employee.EmployeeID);
            }
        }

        public bool CanUpdateEmployee(object parameter)
        {
            bool ret = true;

            if (SelectedEmployee == null)
            {
                Username = "";
                RemainingLeaveDays = "";
                Email = "";
                SelectedRole = EmployeesRole[0];
                IsButtonEnable = true;
                IsTextBoxReadOnly = false;
                IsComboBoxEnable = true;

                ret = false;
            }
            else
            {
                if (SelectedEmployee.IsActive == false)
                {
                    Username = SelectedEmployee.Username;
                    RemainingLeaveDays = SelectedEmployee.RemainingLeaveDays.ToString();
                    Email = SelectedEmployee.Email;
                    SelectedRole = SelectedEmployee.Role.ToString();
                    IsButtonEnable = false;
                    IsTextBoxReadOnly = true;
                    IsComboBoxEnable = false;

                    ret = true;
                }
                else
                {
                    Username = "";
                    RemainingLeaveDays = "";
                    Email = "";
                    SelectedRole = EmployeesRole[0];
                    SelectedRole = EmployeesRole[0];
                    IsButtonEnable = true;
                    IsTextBoxReadOnly = false;
                    IsComboBoxEnable = true;

                    ret = false;
                }
            }

            return ret;
        }

        public void OnDeleteEmployees()
        {
            var selectedRows = AllEmployees.Where(item => item.SelectedCheck).ToList();

            int sum = 0;

            foreach (var row in selectedRows)
            {
                bool result = _employeeService.DeleteById(row.EmployeeID);

                if (result)
                {
                    sum++;
                }
            }

            MessageBox.Show($"Deleted {sum} employee(s)\nSome employees may not have been deleted because they are project managers.");
            this.AllEmployees = (ObservableCollection<Employee>)_employeeService.FindAllEmployees(Employee.EmployeeID);
        }

        public void OnAddNewEmployee(object parameter)
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            string username = Username;
            string password = passwordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(Email)
                || string.IsNullOrEmpty(RemainingLeaveDays))
            {
                MessageBox.Show("Please enter email, username, password and remaining leave days.");
                return;
            }

            if (!Regex.IsMatch(Email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$"))
            {
                MessageBox.Show("Email must be in format: a-z@a-z.a-z");
                return;
            }

            if (!Regex.IsMatch(username, @"^[a-zA-Z\d_]+"))
            {
                MessageBox.Show("For username you can use: a-z, A-z, 0-9 and _");
                return;
            }

            if (!Regex.IsMatch(password, @"[a-zA-Z\d_]+"))
            {
                MessageBox.Show("For password you can use: a-z, A-z, 0-9 and _");
                return;
            }

            if (!Regex.IsMatch(RemainingLeaveDays, @"^\d+$"))
            {
                MessageBox.Show("For remaining days you can use only numbers");
                return;
            }

            if (_employeeService.ExistsByUsername(username))
            {
                MessageBox.Show("Employee with that username already exists.");
                return;
            }

            bool result = _employeeService.AddNewEmployee(username, password, Email, SelectedRole.ToString(), Int32.Parse(RemainingLeaveDays));
            if (result)
            {
                Thread emailThread = new Thread(() =>
                {
                    _emailService.SendEmail(Email, "Account created", $"Your username is: {username} and your password is: {password} . Please update your password in next 48h.");
                });
                emailThread.IsBackground = true;
                emailThread.Start();

                Username = "";
                SelectedRole = EmployeesRole[0];
                RemainingLeaveDays = "";
                Email = "";
                passwordBox.Password = "";
                this.AllEmployees = (ObservableCollection<Employee>)_employeeService.FindAllEmployees(Employee.EmployeeID);
            }
        }
    }
}
