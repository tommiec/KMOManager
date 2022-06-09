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
    /// Interaction logic for CategoryManagement.xaml
    /// </summary>
    public partial class CategoryManagement : Page
    {
        public CategoryManagement()
        {
            InitializeComponent();
            updateDataGridCategories();
        }

        public void updateDataGridCategories()
        {
            using (Context ctx = new Context())
            {
                var queryAllCategories = ctx.Categories.Select(x => x);
                dataCategories.ItemsSource = queryAllCategories.ToList();
            }
        }


        public void updateTextboxesCategories(Category selected)
        {
            if ((Category)selected != null)
            {
                txtName.Text = selected.Name;
                txtDescription.Text = selected.Description;
                chkActiveCategory.IsChecked = selected.IsActive;
            }
            else if ((Category)selected == null)
            {
                txtName.Text = null;
                txtDescription.Text = null;
                chkActiveCategory.IsChecked = false;
            }
        }


        private void dataCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category selected = (Category)dataCategories.SelectedItem;
            updateTextboxesCategories((Category)dataCategories.CurrentItem);
        }

        private void setUserControls (bool enabled)
        {
            txtName.IsEnabled = enabled;
            txtDescription.IsEnabled = enabled;
            chkActiveCategory.IsEnabled = enabled;
        }


        private void btnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            updateTextboxesCategories(null);
            setUserControls(true);
        }


        private void btnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            Category selected = (Category)dataCategories.SelectedItem;


            if (selected != null)
            {
                setUserControls(true);
            }
        }


        private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            Category selected = (Category)dataCategories.SelectedItem;

            if (selected != null)
            {
                using (Context ctx = new Context())
                {
                    ctx.Categories.Remove(selected);
                    ctx.SaveChanges();
                }
                updateDataGridCategories();
            }
        }

        private void btnSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            Category selected = (Category)dataCategories.SelectedItem;

            using (Context ctx = new Context())
            {
                if (selected != null)
                {
                    selected.Name = txtName.Text;
                    selected.Description = txtDescription.Text;
                    selected.IsActive = (bool)chkActiveCategory.IsChecked;
                    ctx.Categories.Update(selected);
                }
                else
                {
                    ctx.Categories.Add(new Category()
                    {
                        Name = txtName.Text,
                        Description = txtDescription.Text,
                        IsActive = (bool)chkActiveCategory.IsChecked
                    });
                }
                ctx.SaveChanges();
            }
            updateDataGridCategories();
            setUserControls(false);
        }
    }
}
