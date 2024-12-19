using AlekseevGlazkiSave.res;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlekseevGlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        private Agent currentAgent = new Agent();
        private ProductSale _currentProductSale = new ProductSale();
        public AddProduct(Agent SelectedAgent)
        {
            InitializeComponent();
            var currentProductSale = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();
            var currentProducts = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();

            currentProductSale = currentProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();

            ProductsComboBox.ItemsSource = currentProducts;
            DataContext = currentProductSale;
        }

        private void ProductsComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProductsComboBox.IsDropDownOpen = true;
            var currentProduct = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();
            currentProduct = currentProduct.Where(p => p.Title.ToLower().Contains(ProductsComboBox.Text.ToLower())).ToList();
            ProductsComboBox.ItemsSource = currentProduct;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();
            _currentProductSale.ID = 0;
            _currentProductSale.AgentID = currentAgent.ID;
            _currentProductSale.ProductID = currentProduct[ProductsComboBox.SelectedIndex].ID;
            _currentProductSale.SaleDate = Convert.ToDateTime(ProductSaleDate.Text);
            _currentProductSale.ProductCount = Convert.ToInt32(ProductCount.Text);

            AlekseevGlazkiSaveEntities.GetContext().ProductSale.Add(_currentProductSale);
            AlekseevGlazkiSaveEntities.GetContext().SaveChanges();


            MessageBox.Show("Информация сохранена");
            Manager.MainFrame.GoBack();
        }
    }
}
