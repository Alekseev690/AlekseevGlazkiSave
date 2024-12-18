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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;

        public AgentPage()
        {
            InitializeComponent();
            var currentAgent = AlekseevGlazkiSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;

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

        public void UpdateAgents()
        {
            var currentAgent = AlekseevGlazkiSaveEntities.GetContext().Agent.ToList();

            currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Phone.Replace("-", " ").Replace("(", "").Replace(")", "").Replace(" ", "").Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (ComboAgentType.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "МФО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ООО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ЗАО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "МКК").ToList();
            }

            if (ComboAgentType.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ОАО").ToList();
            }

            if (ComboAgentType.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ПАО").ToList();
            }

            /////////////////////////////////////////////////////////////////////////////////////

            if (ComboSortType.SelectedIndex == 1)
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();

            if (ComboSortType.SelectedIndex == 2)
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();

            if (ComboSortType.SelectedIndex == 3)
                currentAgent = currentAgent.OrderBy(p => p.Discount).ToList();

            if (ComboSortType.SelectedIndex == 4)
                currentAgent = currentAgent.OrderByDescending(p => p.Discount).ToList();

            if (ComboSortType.SelectedIndex == 5)
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();

            if (ComboSortType.SelectedIndex == 6)
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();


            AgentListView.ItemsSource = currentAgent.ToList();

            AgentListView.ItemsSource = currentAgent;
            TableList = currentAgent;
            ChangePage(0, 0);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean Ifupdate = true;
            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();


                AgentListView.ItemsSource = CurrentPageList;
                AgentListView.Items.Refresh();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AlekseevGlazkiSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = AlekseevGlazkiSaveEntities.GetContext().Agent.ToList();
                UpdateAgents();
            }
        }

        private void EditPriorityBtn_Click(object sender, RoutedEventArgs e)
        {
            int maxPriority = 0;
            foreach (Agent agent in AgentListView.SelectedItems)
            {
                if (agent.Priority > maxPriority)
                    maxPriority = agent.Priority;
            }
            SetWindow myWindow = new SetWindow(maxPriority);
            myWindow.ShowDialog();
            if (string.IsNullOrEmpty(myWindow.TBPriority.Text) || (int.TryParse(myWindow.TBPriority.Text, out int priority) && priority < 0))
                MessageBox.Show("Изменения не произошло");
            else
            {
                int newPriority = Convert.ToInt32(myWindow.TBPriority.Text);
                foreach (Agent agent in AgentListView.SelectedItems)
                {
                    agent.Priority = newPriority;
                }
                try
                {
                    AlekseevGlazkiSaveEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    UpdateAgents();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 1)
                EditPriorityBtn.Visibility = Visibility.Visible;
            else
                EditPriorityBtn.Visibility = Visibility.Hidden;
        }
    }
}
