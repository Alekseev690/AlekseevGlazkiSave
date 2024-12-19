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

namespace AlekseevGlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private Agent currentAgent = new Agent();
        private ProductSale _currentProductSale = new ProductSale();
        public AddProduct(Agent SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null)
                currentAgent = SelectedAgent;

            var currentProductSale = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();

            DataContext = _currentProductSale;

            currentProductSale = currentProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();

            TBoxDateProduct.Text = DateTime.Now.ToString();
            var currentProducts = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();
            currentProducts = currentProducts.Where(p => p.Title.ToLower().Contains(TBoxSearhProduct.Text.ToLower())).ToList();
            ProductComboBox.ItemsSource = currentProducts.Select(p => p.Title);
        }

        public void UpdateSales()
        {
            var currentProductSales = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();

            currentProductSales = currentProductSales.Where(p => p.GetProductName.ToLower().Contains(TBoxSearhProduct.Text.ToLower())).ToList();

            ProductComboBox.ItemsSource = currentProductSales.Select(p => p.GetProductName);
        }

        private void TBoxSearhProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSales();
        }

        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBoxSearhProduct.Text = "";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var productList = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();

            StringBuilder errors = new StringBuilder();

            if (ProductComboBox.SelectedIndex < 0)
                errors.AppendLine("Укажите наименование продукта");

            if (string.IsNullOrWhiteSpace($"{_currentProductSale.ProductCount <= 0}") || _currentProductSale.ProductCount <= 0)
                errors.AppendLine("Укажите количество");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            productList = productList.Where(p => p.Title.ToLower().Contains(ProductComboBox.SelectedItem.ToString().ToLower())).ToList();

            var productID = productList.Select(p => p.ID);



            _currentProductSale.SaleDate = DateTime.Now;
            _currentProductSale.AgentID = currentAgent.ID;
            _currentProductSale.ProductID = ProductComboBox.SelectedIndex + 1;


            AlekseevGlazkiSaveEntities.GetContext().ProductSale.Add(_currentProductSale);

            try
            {
                AlekseevGlazkiSaveEntities.GetContext().SaveChanges();

                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
//AddProductPage