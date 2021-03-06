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
using System.Windows.Shapes;

namespace Warehouse
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        ApplicationContext db;

        public RegWindow()
        {
            InitializeComponent();

        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string pass = passBox.Password;
            string pass2 = passBox2.Password;
            string email = textBoxEmail.Text.Trim().ToLower();

            User regUser = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                regUser = db.Users.Where(b => b.Login == login).FirstOrDefault();
            }
            if (regUser != null)
            {
                textBoxLogin.ToolTip = "Даний логін зайнятий.";
                textBoxLogin.Background = Brushes.DarkRed;
                MessageBox.Show("Даний логін зайнятий.");
            }
            else
            {
                if (login.Length < 5)
                {
                    textBoxLogin.ToolTip = "Логін повинен бути довше 4 символвв!";
                    textBoxLogin.Background = Brushes.DarkRed;
                }
                else if (pass.Length < 5)
                {
                    passBox.ToolTip = "Пароль повинен бути довше 4 символів!";
                    passBox.Background = Brushes.DarkRed;
                }
                else if (pass2 != pass)
                {
                    passBox2.ToolTip = "Паролі не співпадают!";
                    passBox2.Background = Brushes.DarkRed;
                }
                else if (email.Length < 8 || !email.Contains("@") || !email.Contains("."))
                {
                    textBoxEmail.ToolTip = "Це поле введено некоректно! Назва скрині довше 7 символов, наявність @ и . ";
                    textBoxEmail.Background = Brushes.DarkRed;
                }
                else
                {
                    textBoxLogin.ToolTip = "";
                    textBoxLogin.Background = Brushes.Transparent;
                    passBox.ToolTip = "";
                    passBox.Background = Brushes.Transparent;
                    passBox2.ToolTip = "";
                    passBox2.Background = Brushes.Transparent;
                    textBoxEmail.ToolTip = "";
                    textBoxEmail.Background = Brushes.Transparent;

                    User.RegisterUser(login, email, pass);

                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    this.Close();
                }
            }
        }

        private void Button_Return(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
