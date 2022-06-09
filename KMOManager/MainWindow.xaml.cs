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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            switch (Globals.currentUser.Role)
            {
                case Role.Admin:
                    Console.WriteLine("Navigation for Admin-role active");
                    btnDataManagement.Visibility = Visibility.Visible;
                    btnInventory.Visibility = Visibility.Visible;
                    btnSales.Visibility = Visibility.Visible;
                    break;
                case Role.Sales:
                    Console.WriteLine("Navigation for Sales-role active");
                    btnDataManagement.Visibility = Visibility.Hidden;
                    btnInventory.Visibility = Visibility.Hidden;
                    btnSales.Visibility = Visibility.Visible;
                    break;
                case Role.Warehouse:
                    Console.WriteLine("Navigation for Warehouse-role active");
                    btnDataManagement.Visibility = Visibility.Hidden;
                    btnInventory.Visibility = Visibility.Visible;
                    btnSales.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnDataManagement_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new DataManagement();
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Inventory();
        }

        private void btnSales_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Sales();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Globals.currentUser = null;

            Login login = new Login();
            login.Show();
            Close();
            return;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
