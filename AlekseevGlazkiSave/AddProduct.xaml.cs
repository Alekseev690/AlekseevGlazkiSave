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
        private Agent currentAgent = new Agent(); private ProductSale _currentProductSale = new ProductSale();
        DateTime? ProductDate; 
        public AddProduct(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null) currentAgent = SelectedAgent;
            ProductDate = null;
            var currentProducts = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();
            currentProducts = currentProducts.Where(p => p.Title.ToLower().Contains(TBoxSearhProduct.Text.ToLower())).ToList(); ProductComboBox.ItemsSource = currentProducts.Select(p => p.Title);
        }
        public void UpdateProducts()
        {
            var currentProductSales = AlekseevGlazkiSaveEntities.GetContext().Product.ToList();
            currentProductSales = currentProductSales.Where(p => p.Title.ToLower().Contains(TBoxSearhProduct.Text.ToLower())).ToList();
            ProductComboBox.ItemsSource = currentProductSales.Select(p => p.Title);
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            int productCount = 0;
            StringBuilder errors = new StringBuilder();
            if (ProductComboBox.SelectedIndex < 0) errors.AppendLine("Укажите наименование продукта");
            if (ProductDate == null) errors.AppendLine("Укажите дату продукта");
            if (int.TryParse(TBoxCountProduct.Text, out int value))
            {
                if (value <= 0)
                {
                    errors.AppendLine("Количество продукции должно быть больше 0!");
                }
                else
                {
                    productCount = value;
                }
            }
            else
            {
                errors.AppendLine("Значение количества продукции указано неверно!");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString()); return;
            }
            _currentProductSale.AgentID = currentAgent.ID; _currentProductSale.ProductID = ProductComboBox.SelectedIndex + 1;
            _currentProductSale.SaleDate = (DateTime)ProductDate; _currentProductSale.ProductCount = productCount;

            AlekseevGlazkiSaveEntities.GetContext().ProductSale.Add(_currentProductSale);
            try
            {
                AlekseevGlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена"); Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void CloseWindow()
        {
            Close();
        }
        private void TBoxSearhProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }
        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBoxSearhProduct.Text = "";
        }

        private void dtPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductDate = (DateTime)(((DatePicker)sender).SelectedDate);
        }
    }
}
