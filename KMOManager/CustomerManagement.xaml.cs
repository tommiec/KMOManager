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

namespace KMOManager
{
    /// <summary>
    /// Interaction logic for CustomerManagement.xaml
    /// </summary>
    public partial class CustomerManagement : Page
    {
        public CustomerManagement()
        {
            InitializeComponent();
            updateDataGridCustomers();
        }

        public void updateDataGridCustomers()
        {
            using (Context ctx = new Context())
            {
                var queryAllCustomers = ctx.Customers.Select(x => x);
                dataCustomers.ItemsSource = queryAllCustomers.ToList();
            }
        }

        public void updateTextboxesCustomers(Customer selected)
        {
            if ((Customer)selected != null)
            {
                txtName.Text = selected.Name;
                txtFirstName.Text = selected.FirstName;
                txtLastName.Text = selected.LastName;
                txtAddress.Text = selected.Address;
                txtPC.Text = selected.PostalCode;
                txtCity.Text = selected.City;
                txtCountry.Text = selected.Country;
                txtPhone.Text = selected.PhoneNumber;
                txtEmail.Text = selected.Email;
                chkActiveEmployee.IsChecked = selected.IsActive;
            }
            else if ((Customer)selected == null)
            {
                txtName.Text = null;
                txtFirstName.Text = null;
                txtLastName.Text = null;
                txtAddress.Text = null;
                txtPC.Text = null;
                txtCity.Text = null;
                txtCountry.Text = null;
                txtPhone.Text = null;
                txtEmail.Text = null;
                chkActiveEmployee.IsChecked = false;
            }
        }


        private void dataCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = (Customer)dataCustomers.SelectedItem;
            updateTextboxesCustomers((Customer)dataCustomers.CurrentItem);
            Globals.selectedCustomer = selectedCustomer;
        }


        private void setUserControls(bool enabled)
        {
            txtName.IsEnabled = enabled;
            txtFirstName.IsEnabled = enabled;
            txtLastName.IsEnabled = enabled;
            txtAddress.IsEnabled = enabled;
            txtPC.IsEnabled = enabled;
            txtCity.IsEnabled = enabled;
            txtCountry.IsEnabled = enabled;
            txtPhone.IsEnabled = enabled;
            txtEmail.IsEnabled = enabled;
            chkActiveEmployee.IsEnabled = enabled;
        }


        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            updateTextboxesCustomers(null);
            setUserControls(true);
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer selected = (Customer)dataCustomers.SelectedItem;

            if (selected != null)
            {
                setUserControls(true);
            }
        }

        private void btnRemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer selected = (Customer)dataCustomers.SelectedItem;

            if (selected != null)
            {
                using (Context ctx = new Context())
                {
                    ctx.Customers.Remove(selected);
                    ctx.SaveChanges();
                }
                updateDataGridCustomers();
            }
        }

        private void btnSaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customer selected = (Customer)dataCustomers.SelectedItem;

            using (Context ctx = new Context())
            {
                if (selected != null)
                {
                    selected.Name = txtName.Text;
                    selected.FirstName = txtFirstName.Text;
                    selected.LastName = txtLastName.Text;
                    selected.Address = txtAddress.Text;
                    selected.PostalCode = txtPC.Text;
                    selected.City = txtCity.Text;
                    selected.Country = txtCountry.Text;
                    selected.PhoneNumber = txtPhone.Text;
                    selected.Email = txtEmail.Text;
                    selected.IsActive = (bool)chkActiveEmployee.IsChecked;
                    ctx.Customers.Update(selected);
                }
                else
                {
                    ctx.Customers.Add(new Customer()
                    {
                        Name = txtName.Text,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Address = txtAddress.Text,
                        PostalCode = txtPC.Text,
                        City = txtCity.Text,
                        Country = txtCountry.Text,
                        PhoneNumber = txtPhone.Text,
                        Email = txtEmail.Text,
                        IsActive = (bool)chkActiveEmployee.IsChecked
                    });
                }
                ctx.SaveChanges();
            }
            updateDataGridCustomers();
            setUserControls(false);
        }
    }
}
