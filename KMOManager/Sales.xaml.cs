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
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Page
    {
        public Sales()
        {
            InitializeComponent();
        }


        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new CustomerManagement();
        }


        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.selectedCustomer != null)
            {
                Content.Content = new ProductSelection();
            }
            else
            {
                MessageBox.Show("Selecteer eerst een klant aub");
            }
        }


        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.selectedCustomer != null)
            {
                Content.Content = new OrderManagement();
            }
            else
            {
                MessageBox.Show("Selecteer eerst een klant aub");
            }
        }

    }
}
