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

        private void Button_ArrivalProduct(object sender, RoutedEventArgs e)
        {
            ArrivalProductWindow arrivalProductWindow = new ArrivalProductWindow();
            arrivalProductWindow.Show();
            Close();
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            ApplicationContext db = new ApplicationContext();
            List<Product> products = db.Products.ToList();
            List<Product> searchProducts = new List<Product>();
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Name.ToLower().Contains(textBoxSearch.Text.ToLower()))
                {
                    searchProducts.Add(products[i]);
                }
            }
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = searchProducts;
            textBoxSearch.Text = "";
        }

        private void Button_EditingProduct(object sender, RoutedEventArgs e)
        {
            EditingProductWindow editingProductWindow = new EditingProductWindow();
            editingProductWindow.Show();
            Close();
        }
    }
}
