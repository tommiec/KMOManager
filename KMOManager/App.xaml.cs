using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KMOManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>    

    public static class Globals
    {
        public static Customer selectedCustomer { get; set; }
        public static List<Product> ShoppingCart = new List<Product>(); 
        public static Employee currentUser { get; set; }
    }

    public partial class App : Application
    {
    }
}
