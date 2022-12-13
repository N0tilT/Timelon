using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Timelon.App.Core;
using Timelon.Data;

namespace Timelon.App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel = new ApplicationViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            NoCard();

            Title.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LayoutRoot_MouseLeftButtonDown);
            Window_Menu.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LayoutRoot_MouseLeftButtonDown);
        }

        /// <summary>
        /// Перетаскиваение окна по клику мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_MouseLeftButtonDown(object sender, EventArgs e)
        {
            DragMove();
        }
        /// <summary>
        /// Проверка на наличие карточек в списке
        /// </summary>
        private void NoCard()
        {
            if ((viewModel.DefaultCards.Count + viewModel.ImportantCards.Count + viewModel.DoneCards.Count) != 0)
            {
                YesVisible();
            }
            else
            {
                NoVisible();
            }
        }
        /// <summary>
        /// Показать MainCardsMenu и скрыть Veil
        /// </summary>
        void YesVisible()
        {
            MainCardsMenu.Visibility = Visibility.Visible;
            Veil.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Показать Veil и скрыть MainCardsMenu
        /// </summary>
        void NoVisible()
        {
            MainCardsMenu.Visibility = Visibility.Collapsed;
            Veil.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Задержка для проверки нового выбранного списка на наличие карточек
        /// </summary>
        async void Sleeper()
        {
            await Task.Delay(2);
            NoCard();
        }

        #region ButtonClick

        /// <summary>
        /// События скрытия или открытия информации о карте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Sleeper();
            if (CardInfoColumn.Width == new GridLength(240))
                CardInfoColumn.Width = new GridLength(0);
            if (ExtendedCardsMenu.Visibility == Visibility.Visible)
            {
                ExtendedCardsMenu.Visibility = Visibility.Hidden;
                MainCardsMenu.Visibility = Visibility.Visible;
                ExCardInfoColumn.Width = new GridLength(0);
                SearchResult.Visibility = Visibility.Hidden;
                CardListName.Visibility = Visibility.Visible;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if ((viewModel.DefaultCards.Count + viewModel.ImportantCards.Count + viewModel.DoneCards.Count) == 1) NoVisible();
            viewModel.Need_Save = true;
            if (CardInfoColumn.Width == new GridLength(0))
                CardInfoColumn.Width = new GridLength(240);
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (CardInfoColumn.Width == new GridLength(0))
                CardInfoColumn.Width = new GridLength(240);
            DoneCardsPanel.SelectedItem = null;
            CardsPanel.SelectedItem = null;
        }

        private void DoneCardButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Need_Save = true;
        }

        private void ImportantCardButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Need_Save = true;
        }

        private void RecoverCardButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Need_Save = true;
        }

        private void UndoImportantCardButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Need_Save = true;
        }

        private void AddListButton_Click(object sender, RoutedEventArgs e)
        {
            NoVisible();
            viewModel.Need_Save = true;
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            YesVisible();
            viewModel.Need_Save = true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Veil.Visibility = Visibility.Hidden;
            ExtendedCardsMenu.Visibility = Visibility.Visible;
            MainCardsMenu.Visibility = Visibility.Hidden;
            CardInfoColumn.Width = new GridLength(0);
            ExCardInfoColumn.Width = new GridLength(240);
            CardListName.Visibility = Visibility.Collapsed;
            SearchResult.Visibility = Visibility.Visible;
        }

        private void GoToListButton_Click(object sender, RoutedEventArgs e)
        {
            ExtendedCardsMenu.Visibility = Visibility.Hidden;
            MainCardsMenu.Visibility = Visibility.Visible;
            ExCardInfoColumn.Width = new GridLength(0);
            CardListName.Visibility = Visibility.Visible;
            SearchResult.Visibility = Visibility.Collapsed;
        }

        private void DoneCardsShow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DoneCardsPanel.Visibility == Visibility.Hidden)
            {
                CardsPanelArrowDown.Visibility = Visibility.Hidden;
                CardspanelArrowUp.Visibility = Visibility.Visible;
                DoneCardsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                CardspanelArrowUp.Visibility = Visibility.Hidden;
                CardsPanelArrowDown.Visibility = Visibility.Visible;
                DoneCardsPanel.Visibility = Visibility.Hidden;
            }
        }

        #endregion ButtonClick

        #region TextChangedEvents

        //Изменение видимости текстовых полей для полей с шаблонами
        private void SearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TemplateSearch.Visibility = SearchTextbox.Text == "" ? Visibility.Visible : Visibility.Hidden;
        }

        private void AddListTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TemplateList.Visibility = AddListTextbox.Text == "" ? Visibility.Visible : Visibility.Hidden;
        }

        private void AddCardTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TemplateCard.Visibility = AddCardTextbox.Text == "" ? Visibility.Visible : Visibility.Hidden;
        }

        private void CardDateTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardDateTemplate.Visibility = Visibility.Hidden;
        }

        private void CardDescriptionTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardDescriptionTemplate.Visibility = Visibility.Hidden;
        }

        #endregion TextChangedEvents

        #region Window Manager Events

        /// <summary>
        /// Закрыть окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseApp_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (viewModel.Need_Save == true)
            //{
            //    if (MessageBox.Show("Точно хотите выйти? Все несохраненные данные будут удалены.",
            //        "Выход",
            //        MessageBoxButton.YesNo,
            //        MessageBoxImage.Question) == MessageBoxResult.Yes)
            //    {
            //        this.Close();
            //    }
            //}
            //else this.Close();
            this.Hide();
        }

        private void SaveChanges_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            viewModel.Need_Save = false;
            viewModel.ListManager.SaveData();
        }

        /// <summary>
        /// Скрыть окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            Canvas.SetZIndex(CloseButton, 1);
        }

        /// <summary>
        /// Полный экран/оконный режим
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Confirmation();
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.Need_Save = false;
            viewModel.ListManager.SaveData();
        }

        private void Confirmation()
        {
            Window ClearWindow = new Window()
            {
                Visibility = Visibility.Collapsed,
                AllowsTransparency = true,
                Background = System.Windows.Media.Brushes.Transparent,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false
            };
            if (viewModel.Need_Save == true)
            {
                ClearWindow.Show();
                if (MessageBox.Show(ClearWindow, "Точно хотите выйти? Все несохраненные данные будут удалены.",
                    "Выход",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClearWindow.Close();
                    this.Close();
                }
                else ClearWindow.Close();
            }
            else
            {
                ClearWindow.Close();
                this.Close();
            }
        }
        private void Trash_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно хотите удалить весь список?",
            "Удаление",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Trash.Command = viewModel.RemoveListCommand;
            }
            else
            {
                Trash.Command = null;
            }
        }


        #endregion Window Manager Events

        private void AddCardTextbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                YesVisible();
                viewModel.Need_Save = true;
                TextBox tmp = AddCardTextbox;
                if (tmp.Text != "")
                {
                Card newCard = new Card(tmp.Text);
                    viewModel.SelectedList.Set(newCard);
                    viewModel.DefaultCards.Add(newCard);
                    viewModel.SelectedCard = newCard;
                }
            }
        }

        private void SearchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(sender, e);
                TextBox tmp = SearchTextbox;
                if (tmp.Text != "")
                {
                    viewModel._extendedCardList = new List<ExtendedCard>();
                    foreach (KeyValuePair<int, CardList> item in viewModel._listManager.All)
                    {
                        List<Card> searchResult = item.Value.SearchByContent(tmp.Text);
                        foreach (Card card in searchResult)
                            viewModel._extendedCardList.Add(new ExtendedCard(item.Value.Id,
                                item.Value.Name, card.Id, card.Name,
                                card.Date, card.Description, card.IsImportant, card.IsCompleted));
                    }

                    viewModel.ExtendedCards = new ObservableCollection<ExtendedCard>(viewModel._extendedCardList);
                }
            }
        }
    }
}