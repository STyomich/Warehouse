using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ArrivalProductWindow.xaml
    /// </summary>
    public partial class ArrivalProductWindow : Window
    {
        public ArrivalProductWindow()
        {

            InitializeComponent();
            ApplicationContext db = new ApplicationContext();
            List<Product> products = db.Products.ToList();
            List<string> nameProducts = new List<string>();
            for (int i = 0; i < products.Count; i++)
                nameProducts.Add(products[i].Name);
            comboBoxSearchProduct.ItemsSource = nameProducts;
        }

        private void Button_AddInList(object sender, RoutedEventArgs e)
        {
            if (comboBoxSearchProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар.");
            }
            else
            if (textBoxAmount.Text == "")
            {
                MessageBox.Show("Введите количество товара.");
            }
            else
            if (textBoxPrice.Text == "")
            {
                MessageBox.Show("Введите цену товара.");
            }
            else
            if (radioButtonArrival.IsChecked == false && radioButtonShipment.IsChecked == false)
            {
                MessageBox.Show("Выберите один из предложенных операций: Прибытие или Отправка.");
            }
            else
            {
                List<Product> productList = new List<Product>();
                if (dataGrid.ItemsSource != null)
                    productList = (List<Product>)dataGrid.ItemsSource;
                //string name = textBoxSearchProduct.Text;
                string name = comboBoxSearchProduct.Text;
                int amount = Math.Abs(Convert.ToInt32(textBoxAmount.Text));
                int price = Convert.ToInt32(textBoxPrice.Text);

                Product.AddInList(ref productList, name, amount, price, radioButtonArrival, radioButtonShipment);

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = productList;

            }
        }

        private void textBoxAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_generateList(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource == null)
            {
                MessageBox.Show("Лист товарів пуст. Додайте хоча б один товар.");
            }
            else
            {
                List<Product> productPresentList = new List<Product>();
                productPresentList = (List<Product>)dataGrid.ItemsSource;

                Product.GenerateList(productPresentList);

                WorkspaceWindow workspaceWindow = new WorkspaceWindow();
                workspaceWindow.Show();
                Close();
            }
        }
        private void Button_ReturnOnWorkspaceWindow(object sender, RoutedEventArgs e)
        {
            WorkspaceWindow workspaceWindow = new WorkspaceWindow();
            workspaceWindow.Show();
            Close();
        }
    }
}
