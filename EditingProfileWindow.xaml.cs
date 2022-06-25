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
    /// Логика взаимодействия для EditingProfileWindow.xaml
    /// </summary>
    public partial class EditingProfileWindow : Window
    {
        public EditingProfileWindow()
        {
            InitializeComponent();
        }

        private void Button_SaveEditingProfileData(object sender, RoutedEventArgs e)
        {
            ApplicationContext db = new ApplicationContext();
            User user = null;
            user = db.Users.Where(b => b.Login.ToLower() == textBoxNewLogin.Text.ToLower()).FirstOrDefault();
            if (user != null)
            {
                MessageBox.Show("Даний логін зайнятий!");
            }
            else
            {
                string[] userData = File.ReadAllLines("user.txt");
                string newLogin = textBoxNewLogin.Text;
                if (newLogin == "")
                    newLogin = userData[0];

                string newPass;
                if (textBoxNewPass.Password == "")
                    newPass = userData[1];
                else
                    newPass = textBoxNewPass.Password;

                string newEmail;
                if (textBoxNewEmail.Text == "")
                    newEmail = userData[2];
                else
                    newEmail = textBoxNewEmail.Text;

                User checkUser = null;
                string existedLogin = userData[0];
                checkUser = db.Users.Where(b => b.Login.ToLower() == existedLogin.ToLower()).FirstOrDefault();
                checkUser.Login = newLogin;
                checkUser.Password = newPass;
                checkUser.Email = newEmail;

                User.WriteUserData(checkUser);

                db.SaveChanges();
                MessageBox.Show("Done!");

                ProfileWindow profileWindow = new ProfileWindow();
                profileWindow.Show();
                Close();
            }
        }
    }
}
