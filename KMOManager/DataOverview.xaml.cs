using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for DataOverview.xaml
    /// </summary>
    public partial class DataOverview : Page
    {
        public DataOverview()
        {
            InitializeComponent();
        }

        private void btnTopProd_Click(object sender, RoutedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryTopProducts = ctx.OrderDetails
                    .Include(od => od.Product)
                    .GroupBy(p => new { p.Product.ProductId, p.Product.Brand, p.Product.Name })
                    .Select(y => new { ProductId = y.Key.ProductId, Brand = y.Key.Brand, Name = y.Key.Name, Count = y.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                dataOverview.ItemsSource = queryTopProducts.ToList();
            }
        }

        private void btnFlopProd_Click(object sender, RoutedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryFlopProducts = ctx.OrderDetails
                    .Include(od => od.Product)
                    .GroupBy(p => new { p.Product.ProductId, p.Product.Brand, p.Product.Name })
                    .Select(y => new { ProductId = y.Key.ProductId, Brand = y.Key.Brand, Name = y.Key.Name, Count = y.Count() })
                    .OrderBy(x => x.Count)
                    .ToList();

                dataOverview.ItemsSource = queryFlopProducts.ToList();
            }
        }

    }
}
