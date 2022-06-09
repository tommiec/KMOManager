using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KMOManager
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            txtEmail.Focus();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CheckLogin(string email, string password)
        {
            using (Context ctx = new Context())
            {
                try
                {
                    Employee TriedUser = ctx.Employees.Single(x => x.Email.ToLower() == email.ToLower());

                    if ((TriedUser.Email.ToLower() == txtEmail.Text.ToLower()) && (TriedUser.Password == txtPassword.Password))
                    {
                        Globals.currentUser = TriedUser;

                        TriedUser.LastSeen = DateTime.Now;
                        ctx.SaveChanges();

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Foutieve login-gegevens. Probeer opnieuw!");
                        txtEmail.Text = "Email";
                        txtPassword.Password = "Password";
                        txtEmail.Focus();
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show("Oops, dat ging fout. Contacteer Thomas!");
                    txtEmail.Text = "Email";
                    txtPassword.Password = "Password";
                    txtEmail.Focus();
                }
            }


        }

        private void TryLogin()
        {
            if ((txtEmail.Text.Length == 0) || (txtPassword.Password.Length == 0))
            {
                MessageBox.Show("Voer een e-mailadres en wachtwoord in.");
                txtEmail.Focus();
            }
            else
            {
                CheckLogin(txtEmail.Text, txtPassword.Password);
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.SelectAll();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TryLogin();
            }
        }
    }

}
