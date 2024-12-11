﻿using AlekseevGlazkiSave.res;
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

            AgentListView.ItemsSource = currentAgents;
            TableList = currentAgents;
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
    }
}
