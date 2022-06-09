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
    /// Interaction logic for btnsDataManagement.xaml
    /// </summary>
    public partial class DataManagement : Page
    {
        public DataManagement()
        {
            InitializeComponent();
        }

        private void Customers_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new CustomerManagement();
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new EmployeeManagement();
        }


        private void Products_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new ProductManagement();
        }


        private void Audience_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new AudienceManagement();
        }


        private void Category_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new CategoryManagement();
        }

        private void btnOverview_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new DataOverview();
        }
    }
}
