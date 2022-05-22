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
    /// Логика взаимодействия для WorkspaceWindow.xaml
    /// </summary>
    public partial class WorkspaceWindow : Window
    {


        public WorkspaceWindow()
        {
            InitializeComponent();
            ApplicationContext db = new ApplicationContext();
            List<Product> products = db.Products.ToList();
            dataGrid.ItemsSource = products;
            //List<User> users = db.Users.ToList();
            //dataGrid.ItemsSource = users;

        }

        private void Button_Profile(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow();
            profileWindow.Show();
        }

        private void Button_AddCategory(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.Show();
            Close();
        }
    }
}
