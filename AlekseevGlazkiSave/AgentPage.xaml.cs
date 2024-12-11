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
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();
            var currentAgents = AlekseevGlazkiSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgents;

            ComboSortType.ItemsSource = null;
            ComboAgentType.SelectedIndex = 0;

            UpdateAgents();

        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();

        }

        private void ComboSortType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();

        }

        private void ComboAgentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void UpdateAgents()
        {
            var currentAgents = AlekseevGlazkiSaveEntities.GetContext().Agent.ToList();

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Phone.Replace("-", " ").Replace("(", "").Replace(")", "").Replace(" ", "").Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (ComboAgentType.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => p.AgentType.Title == "МФО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => p.AgentType.Title == "ООО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => p.AgentType.Title == "ЗАО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 4)
            {
                currentAgents = currentAgents.Where(p => p.AgentType.Title == "МКК").ToList();
            }

            if (ComboAgentType.SelectedIndex == 5)
            {
                currentAgents = currentAgents.Where(p => p.AgentType.Title == "ОАО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 6)
            {
                currentAgents = currentAgents.Where(p => p.AgentType.Title == "ПАО").ToList();
            }

            /////////////////////////////////////////////////////////////////////////////////////
            
            if (ComboSortType.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }

            if (ComboSortType.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }

            if (ComboSortType.SelectedIndex == 3)
            {
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }

            if (ComboSortType.SelectedIndex == 4)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }

            AgentListView.ItemsSource = currentAgents.ToList();
        }
    }
}
