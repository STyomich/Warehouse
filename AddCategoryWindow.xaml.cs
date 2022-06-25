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

namespace Warehouse
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {

        ApplicationContext db;

        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void Button_AddProduct(object sender, RoutedEventArgs e)
        {
            string name = textBoxName.Text;
            string unit = textBoxUnit.Text;
            int price = 0;
            Product newProduct = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                newProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }
            if (newProduct != null)
            {
                textBoxName.ToolTip = "Даний товар вже існує.";
                textBoxName.Background = Brushes.DarkRed;
                MessageBox.Show("Даный товар вже існує.");
            }
            else
            if (name.Length < 1)
            {
                textBoxName.ToolTip = "Це поле введено некоректно!";
                textBoxName.Background = Brushes.DarkRed;
            }else
            if (unit == "")
            {
                unit = "шт"; 
            }else
            {
                textBoxName.ToolTip = "";
                textBoxName.Background = Brushes.Transparent;
                textBoxUnit.ToolTip = "";
                textBoxUnit.Background = Brushes.Transparent;
                string date_of_last_delivery = "X";
                int amount = 0;
                Product product = new Product(name, unit, price, date_of_last_delivery, amount);
                db.Products.Add(product);
                db.SaveChanges();
                WorkspaceWindow workspaceWindow = new WorkspaceWindow();
                workspaceWindow.Show();
                Close();


            }
        }

        private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_ReturnToWorkspaceWindow(object sender, RoutedEventArgs e)
        {
            WorkspaceWindow workspaceWindow = new WorkspaceWindow();
            workspaceWindow.Show();
            Close();
        }
    }
}
