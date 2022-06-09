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
    /// Interaction logic for ProductSelection.xaml
    /// </summary>
    public partial class ProductSelection : Page
    {
        public ProductSelection()
        {
            InitializeComponent();
            UpdateDataGridsProducts();
            UpdateComboBoxes();
        }


        private void UpdateDataGridsProducts()
        {
            using (Context ctx = new Context())
            {
                var queryAllAvailableProducts = ctx.Products
                    //.Where(p => p.Stock > 0)
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryAllAvailableProducts.ToList();
            }
            DataShoppingCart.ItemsSource = Globals.ShoppingCart.ToList();
        }


        private void UpdateComboBoxes()
        {
            if (DataAvailableProducts != null)
            {
                cboBrand.IsEnabled = true;
                cboName.IsEnabled = true;
                cboSize.IsEnabled = true;
                cboColour.IsEnabled = true;
                cboAudience.IsEnabled = true;
                cboCategory.IsEnabled = true;
                chkStock.IsEnabled = true;
            }


            using (Context ctx = new Context())
            {
                var queryAllBrands = ctx.Products.Select(p => p.Brand).Distinct();
                foreach (var brand in queryAllBrands)
                {
                    cboBrand.Items.Add(brand);
                }

                var queryAllNames = ctx.Products.Select(p => p.Name).Distinct();
                foreach (var name in queryAllNames)
                {
                    cboName.Items.Add(name);
                }

                var queryAllSizes = ctx.Products.Select(p => p.Size).Distinct();
                foreach (var size in queryAllSizes)
                {
                    cboSize.Items.Add(size);
                }

                var queryAllColours = ctx.Products.Select(p => p.Colour).Distinct();
                foreach (var colour in queryAllColours)
                {
                    cboColour.Items.Add(colour);
                }

                var queryAllAudiences = ctx.Products.Select(p => p.Audience).Distinct();
                foreach (var audience in queryAllAudiences)
                {
                    cboAudience.Items.Add(audience);
                }

                var queryAllCategories = ctx.Products.Select(p => p.Category).Distinct();
                foreach (var category in queryAllCategories)
                {
                    cboCategory.Items.Add(category);
                }
            }
        }


        private void cboBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryProductsOnBrands = ctx.Products
                    .Where(p => p.Brand == cboBrand.SelectedItem.ToString())
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryProductsOnBrands.ToList();
            }
        }


        private void cboName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryProductsOnName = ctx.Products
                    .Where(p => p.Name == cboName.SelectedItem.ToString())
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryProductsOnName.ToList();
            }
        }


        private void cboSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryProductsOnSize = ctx.Products
                    .Where(p => p.Size == Convert.ToInt16(cboSize.SelectedItem))
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryProductsOnSize.ToList();
            }
        }


        private void cboColour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryProductsOnColour = ctx.Products
                    .Where(p => p.Colour == cboColour.SelectedItem.ToString())
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryProductsOnColour.ToList();
            }
        }


        private void cboAudience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryProductsOnAudience = ctx.Products
                    .Where(p => p.Audience == cboAudience.SelectedItem)
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryProductsOnAudience.ToList();
            }
        }


        private void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (Context ctx = new Context())
            {
                var queryProductsOnCategory = ctx.Products
                    .Where(p => p.Category == cboCategory.SelectedItem)
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                DataAvailableProducts.ItemsSource = queryProductsOnCategory.ToList();
            }
        }


        private void chkStock_Checked(object sender, RoutedEventArgs e)
        {

            // Checkbox testen bij product met stock = 0
            if (chkStock.IsChecked == true)
            {
                using (Context ctx = new Context())
                {
                    var queryProductsOnStock = ctx.Products
                        .Where(p => p.Stock > 0)
                        .Include(p => p.Category)
                        .Include(p => p.Audience);
                    DataAvailableProducts.ItemsSource = queryProductsOnStock.ToList();
                }
            }
            else
                using (Context ctx = new Context())
                {
                    var queryProductsOnStock = ctx.Products
                        .Where(p => p.Stock >= 0)
                        .Include(p => p.Category)
                        .Include(p => p.Audience);
                    DataAvailableProducts.ItemsSource = queryProductsOnStock.ToList();
                }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Product selected = (Product)DataAvailableProducts.SelectedItem;
            Globals.ShoppingCart.Add(selected);
            DataShoppingCart.ItemsSource = Globals.ShoppingCart.ToList();
        }
    }
}
