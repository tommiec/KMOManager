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
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace KMOManager
{
    /// <summary>
    /// Interaction logic for EmployeeManagement.xaml
    /// </summary>
    public partial class EmployeeManagement : Page
    {
        public EmployeeManagement()
        {
            InitializeComponent();
            UpdateDataGridEmployees();
        }


        public void UpdateDataGridEmployees()
        {
            using (Context ctx = new Context())
            {
                var queryAllEmployees = ctx.Employees.Select(x => x);
                dataEmployees.ItemsSource = queryAllEmployees.ToList();
            }
        }


        public void UpdateTextboxesEmployees(Employee selected)
        {
            if ((Employee)selected != null)
            {
                txtFirstName.Text = selected.FirstName;
                txtLastName.Text = selected.LastName;
                txtEmail.Text = selected.Email;
                txtPassword.Text = selected.Password;
                cboRole.SelectedItem = selected.Role;
                chkActiveEmployee.IsChecked = selected.IsActive;
            }
            else if ((Employee)selected == null)
            {
                txtFirstName.Text = null;
                txtLastName.Text = null;
                txtEmail.Text = null;
                txtPassword.Text = null;
                cboRole.Text = null;
                chkActiveEmployee.IsChecked = false;
            }
        }


        private void LoadComboBoxes(Employee selected)
        {
            if (cboRole.Items.Count == 0)
            {
                foreach (Role role in Enum.GetValues(typeof(Role)))
                {
                    cboRole.Items.Add(role);
                }
            }
        }


        private void DataEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee selected = (Employee)dataEmployees.SelectedItem;

            LoadComboBoxes(selected);
            UpdateTextboxesEmployees((Employee)dataEmployees.CurrentItem);
        }


        private void SetUserControls(bool enabled)
        {
            txtFirstName.IsEnabled = enabled;
            txtLastName.IsEnabled = enabled;
            txtEmail.IsEnabled = enabled;
            txtPassword.IsEnabled = enabled;
            cboRole.IsEnabled = enabled;
            chkActiveEmployee.IsEnabled = enabled;
        }


        private void BtnNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextboxesEmployees(null);
            LoadComboBoxes(null);
            SetUserControls(true);
        }


        private void BtnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = (Employee)dataEmployees.SelectedItem;

            if (selected != null)
            {
                SetUserControls(true);
            }
        }


        private void BtnRemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = (Employee)dataEmployees.SelectedItem;

            if (selected != null)
            {
                using (Context ctx = new Context())
                {
                    ctx.Employees.Remove(selected);
                    ctx.SaveChanges();
                }
                UpdateDataGridEmployees();
            }
        }


        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = (Employee)dataEmployees.SelectedItem;

            using (Context ctx = new Context())
            {
                if (selected != null)
                {
                    selected.FirstName = txtFirstName.Text;
                    selected.LastName = txtLastName.Text;
                    selected.Email = txtEmail.Text;
                    selected.Password = txtPassword.Text;
                    selected.Role = (Role)cboRole.SelectedItem;
                    selected.IsActive = (bool)chkActiveEmployee.IsChecked;
                    ctx.Employees.Update(selected);
                }
                else
                {
                    ctx.Employees.Add(new Employee()
                    {
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Email = txtEmail.Text,
                        Password = txtPassword.Text,
                        Role = (Role)cboRole.SelectedItem,
                        IsActive = (bool)chkActiveEmployee.IsChecked
                    });
                }
                ctx.SaveChanges();
            }
            UpdateDataGridEmployees();
            SetUserControls(false);
        }
    }
}
