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
    /// Логика взаимодействия для Realize.xaml
    /// </summary>
    public partial class Realize : Page
    {
        private Agent currentAgent = new Agent();
        public Realize(Agent SelectedAgent)
        {
            InitializeComponent();
            currentAgent = SelectedAgent;

            if (SelectedAgent != null)
            {
                currentAgent = SelectedAgent;
            }

            var currentSales = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();

            currentSales = currentSales.Where(p => p.AgentID == currentAgent.ID).ToList();
            SalesListView.ItemsSource = currentSales;

            DeleteSale.Visibility = Visibility.Collapsed;
            UpdateSales();
        }

        private void UpdateSales()
        {
            var currentProductSales = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();
            currentProductSales = currentProductSales.Where(p => p.AgentID == currentAgent.ID).ToList();

            SalesListView.ItemsSource = currentProductSales;
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AlekseevGlazkiSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SalesListView.ItemsSource = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();
                UpdateSales();
            }
        }

        private void SalesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalesListView.SelectedItems.Count == 0)
                DeleteSale.Visibility = Visibility.Collapsed;
            if (SalesListView.SelectedItems.Count > 0)
                DeleteSale.Visibility = Visibility.Visible;
            UpdateSales();
            SalesListView.Items.Refresh();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddProduct(currentAgent));
            UpdateSales();
            SalesListView.Items.Refresh();
        }

        private void DeleteSale_Click(object sender, RoutedEventArgs e)
        {
            if (SalesListView.SelectedItems.Count < 1)
            {
                MessageBox.Show("Удаление невозможно! Выберите хотя бы одну реализацию!");
            }
            else
            {
                if (MessageBox.Show($"Вы точно хотите выполнить удаление {SalesListView.SelectedItems.Count} реализаций?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        List<ProductSale> productSale = new List<ProductSale>(SalesListView.SelectedItems.OfType<ProductSale>());
                        foreach (ProductSale productSaleItem in productSale)
                        {
                            AlekseevGlazkiSaveEntities.GetContext().ProductSale.Remove(productSaleItem);
                        }
                        AlekseevGlazkiSaveEntities.GetContext().SaveChanges();

                        UpdateSales();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
    }
}
