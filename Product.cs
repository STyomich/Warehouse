using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Warehouse
{
    internal class Product
    {
        public int id { get; set; }

        public string name;

        public string unit;

        public int price;

        public string date_of_last_delivery;

        public int amount;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Date_of_last_delivery
        {
            get { return date_of_last_delivery; }
            set { date_of_last_delivery = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Product () { }

        public static void GenerateList(List<Product> productPresentList)
        {
            ApplicationContext db = new ApplicationContext();

            bool shipmentProducts = false;

            bool arrivalProducts = false;

            for (int i = 0; i < productPresentList.Count; i++)
            {
                Product addedProduct = null;
                string name = productPresentList[i].Name;
                addedProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
                if (addedProduct != null)
                {
                    addedProduct = db.Products.Find(addedProduct.id);
                    addedProduct.amount += productPresentList[i].amount;

                    if (productPresentList[i].amount < 0)
                        shipmentProducts = true;
                    if (productPresentList[i].amount > 0)
                        arrivalProducts = true;

                    if (productPresentList[i].amount > 0)
                    {
                        addedProduct.Date_of_last_delivery = DateTime.Today.ToShortDateString();
                        addedProduct.Price = productPresentList[i].price;
                    }
                    db.SaveChanges();
                }
                else
                    MessageBox.Show("Помилка! Такого продукту нема у списку, додайте його у вікні додавання.");
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Текстовий файл (*.txt)|*.txt";

            if (shipmentProducts == true)
            {
                MessageBox.Show("Формування видаткової накладної.");
                if (dlg.ShowDialog() == true)
                {
                    int sumShipment = 0;
                    File.AppendAllText(dlg.FileName, "--ВИДАТКОВА НАКЛАДНА--");
                    File.AppendAllText(dlg.FileName, "\nТовари на відправку:");
                    for (int i = 0; i < productPresentList.Count; i++)
                    {
                        if (productPresentList[i].amount < 0)
                        {
                            File.AppendAllText(dlg.FileName, "\n | " + productPresentList[i].name + " у кількості " + -productPresentList[i].amount + " " + productPresentList[i].unit + ".");
                            sumShipment += productPresentList[i].amount * productPresentList[i].price;
                        }
                    }
                    File.AppendAllText(dlg.FileName, "\nЗагальна вартість продажу: " + -sumShipment + " грн.");

                    File.AppendAllText(dlg.FileName, "\n-----------");
                    string[] userData = File.ReadAllLines("user.txt");
                    File.AppendAllText(dlg.FileName, "\nРобітник на зміні: " + userData[0]);
                    File.AppendAllText(dlg.FileName, "\nE-mail: " + userData[2]);
                }
            }
            if (arrivalProducts == true)
            {
                MessageBox.Show("Формування прибуткової накладної.");
                if (dlg.ShowDialog() == true)
                {
                    int sumArrival = 0;
                    File.AppendAllText(dlg.FileName, "--ПРИБУТКОВА НАКЛАДНА--");
                    File.AppendAllText(dlg.FileName, "\nТовари на отримання:");
                    for (int i = 0; i < productPresentList.Count; i++)
                    {
                        if (productPresentList[i].amount > 0)
                        {
                            File.AppendAllText(dlg.FileName, "\n | " + productPresentList[i].name + " у кількості " + productPresentList[i].amount + " " + productPresentList[i].unit + ".");
                            sumArrival += productPresentList[i].amount * productPresentList[i].price;
                        }
                    }
                    File.AppendAllText(dlg.FileName, "\nЗагальна вартість покупки: " + sumArrival + " грн.");

                    File.AppendAllText(dlg.FileName, "\n-----------");
                    string[] userData = File.ReadAllLines("user.txt");
                    File.AppendAllText(dlg.FileName, "\nРобітник на зміні: " + userData[0]);
                    File.AppendAllText(dlg.FileName, "\nE-mail: " + userData[2]);
                }
            }
        }
        public static void AddInList(ref List<Product> productList, string name, int amount, int price, RadioButton radioButtonArrival, RadioButton radioButtonShipment)
        {
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
                                MessageBox.Show("Помилка! Кількість відправленого товару первіщує кількість у маєтку.");
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
                Product product = new Product(name, "кг", price, "01.01.2022", amount);
                if (radioButtonShipment.IsChecked == true)
                    product.amount = -amount;
                Product checkProduct = null;
                ApplicationContext db = new ApplicationContext();
                checkProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
                if (checkProduct != null)
                {
                    product.Unit = checkProduct.Unit;
                    if (amount > 0)
                        product.Date_of_last_delivery = DateTime.Today.ToShortDateString();
                    if (checkProduct.Amount + product.Amount < 0)
                        MessageBox.Show("Помилка! Кількість відправленого товару первіщує кількість у маєтку.");
                    else
                    {
                        checkProduct.Price = product.Price;
                        productList.Add(product);
                    }
                }
                else
                    MessageBox.Show("Даного товару немає. Додати його на головному екрані.");
            }
        }
        public Product(string name, string unit, int price, string date_of_last_delivery, int amount)
        {
            this.name = name;
            this.unit = unit;
            this.price = price;
            this.date_of_last_delivery = date_of_last_delivery;
            this.amount = amount;
        }

    }
}
