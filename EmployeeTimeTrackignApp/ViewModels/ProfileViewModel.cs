using EmployeeTimeTrackignApp.Helpers;
using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeTimeTrackignApp.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly EmployeeService _employeeService = new EmployeeService();
        private Employee Employee { get; set; }

        private string _accountStatus;
        public string AccountStatus
        {
            get { return _accountStatus; }
            set
            {
                SetProperty(ref _accountStatus, value);
            }
        }

        private string _accountRole;
        public string AccountRole
        {
            get { return _accountRole; }
            set
            {
                SetProperty(ref _accountRole, value);
            }
        }

        private string _accountRemainingLeaveDays;
        public string AccountRemainingLeaveDays
        {
            get { return _accountRemainingLeaveDays; }
            set
            {
                SetProperty(ref _accountRemainingLeaveDays, value);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
                //UpdateAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
                //UpdateAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public MyICommand<object> UpdateAccountCommand { get; set; }
        public ProfileViewModel(Employee employee)
        {
            Employee = _employeeService.FindByUsername(employee.Username, employee.Password);
            if (Employee.IsActive == true)
            {
                AccountStatus = "Active";
            }
            else
            {
                AccountStatus = "Inactive";
            }
            AccountRole = Employee.Role.ToString();
            AccountRemainingLeaveDays = Employee.RemainingLeaveDays.ToString();
            Email = Employee.Email;
            Username = Employee.Username;

            //Ne moze sa provjerom jer ne mogu da izvucem podatke iz PasswordBox-a i aktiviram komandu
            //UpdateAccountCommand = new MyICommand<object>(UpdateAccount, CanUpdateProfile);
            UpdateAccountCommand = new MyICommand<object>(UpdateAccount);
        }

        public void UpdateAccount(object parameter)
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(passwordBox.Password) || string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Please enter email, username and password.");
                return;
            }

            if (!Regex.IsMatch(Email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$"))
            {
                MessageBox.Show("Email must be in format: a-z@a-z.a-z");
                return;
            }

            if (!Regex.IsMatch(Username, @"^[a-zA-Z\d_]+"))
            {
                MessageBox.Show("For username you can use: a-z, A-z, 0-9 and _");
                return;
            }

            if (!Regex.IsMatch(passwordBox.Password, @"[a-zA-Z\d_]+"))
            {
                MessageBox.Show("For password you can use: a-z, A-z, 0-9 and _");
                return;
            }

            try
            {
                Employee.Email = Email;
                Employee.Username = Username;
                Employee.Password = passwordBox.Password;
                Employee.PasswordUpdated = true;

                bool result = _employeeService.Update(Employee);
                if (result == true)
                {
                    MessageBox.Show("Successfully updated profile.");
                }
                else
                {
                    MessageBox.Show("Failed while trying to update your profile.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //public bool CanUpdateProfile(object parameter)
        //{
        //    bool ret = true;

        //    if (parameter is not PasswordBox passwordBox)
        //        return false;

        //    if (!Regex.IsMatch(Email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$"))
        //    {
        //        ret = false;
        //    }

        //    if (!Regex.IsMatch(Username, @"^[a-zA-Z\d\.-]+"))
        //    {
        //        ret = false;
        //    }

        //    if (!Regex.IsMatch(passwordBox.Password, @"[a-zA-Z\d_]+"))
        //    {
        //        ret = false;
        //    }

        //    return ret;
        //}
    }
}
