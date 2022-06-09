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
    /// Interaction logic for ProductManagement.xaml
    /// </summary>
    public partial class ProductManagement : Page
    {
        public ProductManagement()
        {
            InitializeComponent();
            UpdateDataGridProducts();
        }


        public void UpdateDataGridProducts()
        {
            using (Context ctx = new Context())
            {
                var queryAllProducts = ctx.Products.Select(x => x)
                    .Include(p => p.Category)
                    .Include(p => p.Audience);
                dataProducts.ItemsSource = queryAllProducts.ToList();
            }
        }


        public void UpdateTextboxesProducts(Product selected)
        {
            if (selected != null)
            {
                txtBrand.Text = selected.Brand;
                txtName.Text = selected.Name;
                txtSize.Text = Convert.ToString(selected.Size);
                txtColour.Text = selected.Colour;
                txtPrice.Text = selected.Price.ToString();
                txtStock.Text = Convert.ToString(selected.Price);
                chkActiveProduct.IsChecked = selected.IsActive;
            }
            else if (selected == null)
            {
                txtBrand.Text = null;
                txtName.Text = null;
                txtSize.Text = null;
                txtColour.Text = null;
                cboAudience.Text = null;
                cboCategory.Text = null;
                txtPrice.Text = null;
                txtStock.Text = null;
                chkActiveProduct.IsChecked = false;
            }
        }

        private void LoadComboBoxes(Product selected)
        {
            using (Context ctx = new Context())
            {
                var queryAllAudiences = ctx.Audiences.Select(x => x);
                cboAudience.ItemsSource = queryAllAudiences.ToList();

                var queryAllCategories = ctx.Categories.Select(x => x);
                cboCategory.ItemsSource = queryAllCategories.ToList();

                if (selected != null)
                {
                    cboAudience.Text = ((Audience)selected.Audience).ToString();
                    cboCategory.Text = ((Category)selected.Category).ToString();
                }
            }
        }


        private void DataProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product selected = (Product)dataProducts.SelectedItem;

            LoadComboBoxes(selected);
            UpdateTextboxesProducts(selected);
        }


        private void SetUserControls(bool enabled)
        {
            txtBrand.IsEnabled = enabled;
            txtName.IsEnabled = enabled;
            txtSize.IsEnabled = enabled;
            txtColour.IsEnabled = enabled;
            cboAudience.IsEnabled = enabled;
            cboCategory.IsEnabled = enabled;
            txtPrice.IsEnabled = enabled;
            txtStock.IsEnabled = enabled;
            chkActiveProduct.IsEnabled = enabled;
        }


        private void BtnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextboxesProducts(null);
            LoadComboBoxes(null);
            SetUserControls(true);
        }


        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selected = (Product)dataProducts.SelectedItem;

            if (selected != null)
            {
                SetUserControls(true);
            }
        }


        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selected = (Product)dataProducts.SelectedItem;

            if (selected != null)
            {
                using (Context ctx = new Context())
                {
                    ctx.Products.Remove(selected);
                    ctx.SaveChanges();
                }
                UpdateDataGridProducts();
            }
        }

        private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selected = (Product)dataProducts.SelectedItem;

            try
            {
                using (Context ctx = new Context())
                {
                    if (selected != null)
                    {
                        selected.Brand = txtBrand.Text;
                        selected.Name = txtName.Text;
                        selected.Size = Convert.ToInt32(txtSize.Text);
                        selected.Colour = txtColour.Text;
                        selected.Audience = (Audience)cboAudience.SelectedItem;
                        selected.Category = (Category)cboCategory.SelectedItem;
                        selected.Price = Convert.ToDouble(txtPrice.Text);
                        selected.Stock = Convert.ToInt32(txtStock.Text);
                        selected.IsActive = (bool)chkActiveProduct.IsChecked;
                        ctx.Products.Update(selected);
                    }
                    else
                    {
                        ctx.Products.Add(new Product()
                        {
                            Brand = txtBrand.Text,
                            Name = txtName.Text,
                            Size = Convert.ToInt16(txtSize.Text),
                            Colour = txtColour.Text,
                            AudienceId = ((Audience)cboAudience.SelectedItem).AudienceId,
                            CategoryId = ((Category)cboCategory.SelectedItem).CategoryId,
                            Price = Convert.ToDouble(txtPrice.Text),
                            Stock = Convert.ToInt32(txtStock.Text),
                            IsActive = (bool)chkActiveProduct.IsChecked
                        });
                    }
                    ctx.SaveChanges();

                }

            }
            catch (Exception)
            {
                MessageBox.Show("Oops, er ging iets fout. Vraag het aan Thomas");
            }

            UpdateDataGridProducts();
            SetUserControls(false);
        }

    }
}
