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
using System.Windows.Shapes;

namespace EmployeeTimeTrackignApp.Views
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Window
    {
        public EmployeeView(EmployeeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            var viewModel1 = (EmployeeViewModel)this.DataContext;
            viewModel1.CloseWindowAction = new Action(this.Close);
        }
    }
}
