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
    /// Логика взаимодействия для ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            string[] userData = File.ReadAllLines("user.txt");
            InitializeComponent();
            profileLogin.Text = userData[0];
            profilePass.Text = userData[1];
            profileEmail.Text = userData[2];
        }

        private void Button_EditingProfileWindow(object sender, RoutedEventArgs e)
        {
            EditingProfileWindow editingProfileWindow = new EditingProfileWindow();
            editingProfileWindow.Show();
            Close();
        }
    }
}
