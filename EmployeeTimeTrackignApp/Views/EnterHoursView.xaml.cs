using EmployeeTimeTrackignApp.Models;
using EmployeeTimeTrackignApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeTimeTrackignApp.Views
{
    /// <summary>
    /// Interaction logic for EnterHoursView.xaml
    /// </summary>
    public partial class EnterHoursView : UserControl
    {
        public EnterHoursView(Employee employee)
        {
            InitializeComponent();
            DataContext = new EnterHoursViewModel(employee);
        }
    }
}
