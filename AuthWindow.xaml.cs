using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        public void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string pass = passBox.Password;

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "Это поле введено не коректно!";
                textBoxLogin.Background = Brushes.DarkRed;
            }
            else if (pass.Length < 5)
            {
                passBox.ToolTip = "Это поле введено не коректно!";
                passBox.Background = Brushes.DarkRed;
            } else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                User authUser = null;
                using (ApplicationContext db = new ApplicationContext())
                {
                    authUser = db.Users.Where(b => b.Login == login && b.Password == pass).FirstOrDefault();
                }
                if (authUser != null)
                {
                    MessageBox.Show("Done");
                    //UserPageWindow userPageWindow = new UserPageWindow();
                    //userPageWindow.Show();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Close();
                    WorkspaceWindow workspaceWindow = new WorkspaceWindow();
                    workspaceWindow.Show();

                    User.WriteUserData(authUser);

                    /*File.WriteAllText("user.txt", "");
                    File.AppendAllText("user.txt", authUser.Login);
                    File.AppendAllText("user.txt", "\n"+authUser.Password);
                    File.AppendAllText("user.txt", "\n"+authUser.Email);*/

                    Close();
                }
                else
                    MessageBox.Show("Ошибка! Введены неверные данные.");
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
