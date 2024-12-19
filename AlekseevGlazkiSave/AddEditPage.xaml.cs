using AlekseevGlazkiSave.res;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private AgentPage agentPage = new AgentPage();
        private Agent currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null)
            {
                currentAgent = SelectedAgent;
            }

            DataContext = currentAgent;

            ComboType.ItemsSource = AlekseevGlazkiSaveEntities.GetContext().AgentType.ToList();
            ComboType.DisplayMemberPath = "Title";
            ComboType.SelectedValuePath = "ID";
            ComboType.SelectedValue = currentAgent.AgentTypeID;
        }

        private void ChangePictureButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentAgent.Logo = myOpenFileDialog.FileName;
                Logo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(currentAgent.Title))
            {
                errors.AppendLine("Укажите наименоваание агента");
            }

            if (string.IsNullOrWhiteSpace(currentAgent.Address))
            {
                errors.AppendLine("Укажите адрес агента");
            }

            if (string.IsNullOrWhiteSpace(currentAgent.DirectorName))
            {
                errors.AppendLine("Укажите ФИО директора");
            }

            if (ComboType.SelectedItem == null)
            {
                errors.AppendLine("Укажите тип агента");
            }

            if (string.IsNullOrWhiteSpace(currentAgent.Priority.ToString()))
            {
                errors.AppendLine("Укажите приоритет агента");
            }

            if (currentAgent.Priority <= 0)
            {
                errors.AppendLine("Укажите положительный проиритет агента");
            }

            if (string.IsNullOrWhiteSpace(currentAgent.INN))
            {
                errors.AppendLine("Укажите ИНН агента");
            }

            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
            {
                errors.AppendLine("Укажите КПП агента");
            }

            if (string.IsNullOrWhiteSpace(currentAgent.Phone))
            {
                errors.AppendLine("Укажите телефон агента");
            }
            else
            {
                string ph = currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) || (ph[1] == '3' && ph.Length != 12) )
                {
                    errors.AppendLine("Укажите правильно телефон агента");
                }

                if (string.IsNullOrWhiteSpace(currentAgent.Email))
                    errors.AppendLine("Укажите почту агента");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                if (currentAgent.ID == 0)
                    AlekseevGlazkiSaveEntities.GetContext().Agent.Add(currentAgent);

                try
                {
                    AlekseevGlazkiSaveEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex) { 
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;

            var currentProductSale = AlekseevGlazkiSaveEntities.GetContext().ProductSale.ToList();
            currentProductSale = currentProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();

            if (currentProductSale.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AlekseevGlazkiSaveEntities.GetContext().Agent.Remove(currentAgent);
                        AlekseevGlazkiSaveEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RealizeButton_Click(object sender, RoutedEventArgs e)
        {
            Realize window = new Realize(currentAgent);
            window.ShowDialog();
        }
    }
}
