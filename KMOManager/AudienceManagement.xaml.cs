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
    /// Interaction logic for AudienceManagement.xaml
    /// </summary>
    public partial class AudienceManagement : Page
    {
        public AudienceManagement()
        {
            InitializeComponent();
            updateDataGridAudiences();
        }

        public void updateDataGridAudiences()
        {
            using (Context ctx = new Context())
            {
                var queryAllAudiences = ctx.Audiences.Select(x => x);
                dataAudiences.ItemsSource = queryAllAudiences.ToList();
            }
        }


        public void updateTextboxesAudiences(Audience selected)
        {
            if ((Audience)selected != null)
            {
                txtName.Text = selected.Name;
                chkActiveAudience.IsChecked = selected.IsActive;
            }
            else if ((Audience)selected == null)
            {
                txtName.Text = null;
                chkActiveAudience.IsChecked = false;
            }
        }


        private void dataAudiences_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Audience selected = (Audience)dataAudiences.SelectedItem;
            updateTextboxesAudiences((Audience)dataAudiences.CurrentItem);
        }


        private void setUserControls(bool enabled)
        {
            txtName.IsEnabled = enabled;
            chkActiveAudience.IsEnabled = enabled;
        }

        private void btnNewAudience_Click(object sender, RoutedEventArgs e)
        {
            updateTextboxesAudiences(null);
            setUserControls(true);
        }


        private void btnEditAudience_Click(object sender, RoutedEventArgs e)
        {
            Audience selected = (Audience)dataAudiences.SelectedItem;

            if (selected != null)
            {
                setUserControls(true);
            }
        }


        private void btnRemoveAudience_Click(object sender, RoutedEventArgs e)
        {
            Audience selected = (Audience)dataAudiences.SelectedItem;

            if (selected != null)
            {
                using (Context ctx = new Context())
                {
                    ctx.Audiences.Remove(selected);
                    ctx.SaveChanges();
                }
                updateDataGridAudiences();
            }

        }

        private void btnSaveAudience_Click(object sender, RoutedEventArgs e)
        {
            Audience selected = (Audience)dataAudiences.SelectedItem;

            using (Context ctx = new Context())
            {
                if (selected != null)
                {
                    selected.Name = txtName.Text;
                    selected.IsActive = (bool)chkActiveAudience.IsChecked;
                    ctx.Audiences.Update(selected);
                }
                else
                {
                    ctx.Audiences.Add(new Audience()
                    {
                        Name = txtName.Text,
                        IsActive = (bool)chkActiveAudience.IsChecked
                    });
                }
                ctx.SaveChanges();
            }
            updateDataGridAudiences();
            setUserControls(false);
        }
    }
}
