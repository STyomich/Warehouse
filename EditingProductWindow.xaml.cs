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
    /// Логика взаимодействия для EditingProductWindow.xaml
    /// </summary>
    public partial class EditingProductWindow : Window
    {
        public EditingProductWindow()
        {
            InitializeComponent();
            ApplicationContext db = new ApplicationContext();
            List<Product> products = db.Products.ToList();
            List<string> nameProducts = new List<string>();
            for (int i = 0; i < products.Count; i++)
                nameProducts.Add(products[i].Name);
            comboBoxSearchProduct.ItemsSource = nameProducts;
        }

        private void Button_ReturnToWorkspaceWindow(object sender, RoutedEventArgs e)
        {
            WorkspaceWindow workspaceWindow = new WorkspaceWindow();
            workspaceWindow.Show();
            Close();
        }

        private void Button_EditProduct(object sender, RoutedEventArgs e)
        {
            if (comboBoxSearchProduct.SelectedItem == null)
            {
                MessageBox.Show("Оберіть товар для редагування.");
            }
            else
            {
                string name = comboBoxSearchProduct.Text;
                string newName = textBoxName.Text;
                if (newName == "")
                    newName = name;
                string newUnit = textBoxUnit.Text;
                Product checkProduct = null;
                ApplicationContext db = new ApplicationContext();
                    checkProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
                if (newUnit == "")
                    newUnit = checkProduct.Unit;
                checkProduct.name = newName;
                checkProduct.unit = newUnit;
                db.SaveChanges();
                MessageBox.Show("Done!");
            }
        }

        private void Button_DeleteProduct(object sender, RoutedEventArgs e)
        {
            if (comboBoxSearchProduct.SelectedItem == null)
            {
                MessageBox.Show("Оберіть товар для редавгування.");
            }
            else
            {
                string name = comboBoxSearchProduct.Text;
                Product checkProduct = null;
                using (ApplicationContext db = new ApplicationContext())
                {
                    checkProduct = db.Products.Where(b => b.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    db.Products.Remove(checkProduct);
                    db.SaveChanges();
                }
                MessageBox.Show("Done!");
                WorkspaceWindow workspaceWindow = new WorkspaceWindow();
                workspaceWindow.Show();
                Close();
            }
        }
    }
}
