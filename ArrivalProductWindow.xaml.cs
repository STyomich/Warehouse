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
        }

        private void Button_AddInList(object sender, RoutedEventArgs e)
        {
            if (textBoxSearchProduct.Text == "")
            {
                MessageBox.Show("Введите название товара.");
            }
            else
            if(textBoxAmount.Text == "")
            {
                MessageBox.Show("Введите количество товара.");
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
                string name = textBoxSearchProduct.Text;
                int amount = Math.Abs(Convert.ToInt32(textBoxAmount.Text));
                bool productExist = false;
                for (int i = 0; i < productList.Count; i++)
                {
                    if (name.ToLower() == productList[i].Name.ToLower())
                    {
                        if (radioButtonArrival.IsChecked == true)
                        {
                            productList[i].Date_of_last_delivery = DateTime.Today.ToShortDateString();
                            if (productList[i].Amount < 0)
                                productList[i].Amount = -productList[i].Amount;
                            else
                                productList[i].Amount = amount;
                        }
                        if (radioButtonShipment.IsChecked == true)
                        {
                            if (amount > 0)
                            {
                                Product checkProduct = null;
                                using (ApplicationContext db = new ApplicationContext())
                                {
                                    checkProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
                                }
                                if (checkProduct.Amount - amount < 0)
                                    MessageBox.Show("Ошибка! Количество отправленного товара превышает количество в имении.");
                                else
                                {
                                    productList[i].amount = -amount;
                                    productList[i].Date_of_last_delivery = checkProduct.Date_of_last_delivery;
                                }
                            }
                        }
                        productExist = true;
                        break;
                    }
                }
                if (!productExist)
                {
                    Product product = new Product(name, "кг", 1, "01.01.2022", amount);
                    if (radioButtonShipment.IsChecked == true)
                        product.amount = -amount;
                    Product checkProduct = null;
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        checkProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    }
                    if (checkProduct != null)
                    {
                        product.Unit = checkProduct.Unit;
                        if(amount > 0)
                            product.Date_of_last_delivery = DateTime.Today.ToShortDateString();
                        if (checkProduct.Amount + product.Amount < 0)
                            MessageBox.Show("Ошибка! Количество отправленного товара превышает количество в имении.");
                        else
                        {
                            product.Price = checkProduct.Price;
                            productList.Add(product);
                        }
                    }
                    else
                        MessageBox.Show("Данного товара не существует. Добавьте его на главном экране.");
                }
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
                MessageBox.Show("Список товаров пуст. Добавьте хотя бы один товар.");
            }
            else
            {
                List<Product> productPresentList = new List<Product>();
                productPresentList = (List<Product>)dataGrid.ItemsSource;

                ApplicationContext db = new ApplicationContext();
                //List<Product> productDBList = db.Products.ToList();

                for (int i = 0; i < productPresentList.Count; i++)
                {
                    Product addedProduct = null;
                    string name = productPresentList[i].Name;
                    addedProduct = db.Products.Where(b => b.Name.ToLower() == name).FirstOrDefault();
                    if (addedProduct != null)
                    {
                        addedProduct = db.Products.Find(addedProduct.id);
                        addedProduct.amount += productPresentList[i].amount;
                        if (productPresentList[i].amount > 0)
                            addedProduct.Date_of_last_delivery = DateTime.Today.ToShortDateString();
                        db.SaveChanges();
                    }
                    else
                        MessageBox.Show("Ошибка! Такого продукта нет в списке, добавьте его на главном экране.");
                }
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Текстовый файл (*.txt)|*.txt";
                if(dlg.ShowDialog() == true)
                {
                    int sumShipment = 0;
                    int sumArrival = 0;
                    File.AppendAllText(dlg.FileName, "--НАКЛАДНАЯ--");
                    File.AppendAllText(dlg.FileName, "\nТовары на отправку:");
                    for (int i = 0; i < productPresentList.Count; i++)
                    {
                        if (productPresentList[i].amount < 0)
                        {
                            File.AppendAllText(dlg.FileName, "\n | " + productPresentList[i].name + " в количестве " + -productPresentList[i].amount + " " + productPresentList[i].unit + ".");
                            sumShipment += productPresentList[i].amount * productPresentList[i].price;
                        }
                    }
                    File.AppendAllText(dlg.FileName, "\nОбщая стоимость продажи: " + -sumShipment + " грн.");
                    File.AppendAllText(dlg.FileName, "\n-----------");
                    File.AppendAllText(dlg.FileName, "\nТовары на получение:");
                    for (int i = 0; i < productPresentList.Count; i++)
                    {
                        if (productPresentList[i].amount > 0)
                        {
                            File.AppendAllText(dlg.FileName, "\n | " + productPresentList[i].name + " в количестве " + productPresentList[i].amount + " " + productPresentList[i].unit + ".");
                            sumArrival += productPresentList[i].amount * productPresentList[i].price;
                        }
                    }
                    File.AppendAllText(dlg.FileName, "\nОбщая стоимость покупки: " + sumArrival + " грн.");
                    File.AppendAllText(dlg.FileName, "\n-----------");
                    string[] userData = File.ReadAllLines("user.txt");
                    File.AppendAllText(dlg.FileName, "\nРабочий на смене: " + userData[0]);
                    File.AppendAllText(dlg.FileName, "\nE-mail: " + userData[2]);
                    MessageBox.Show("Done");
                    WorkspaceWindow workspaceWindow = new WorkspaceWindow();
                    workspaceWindow.Show();
                    Close();
                }
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
