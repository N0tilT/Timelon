using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Timelon.App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel = new ApplicationViewModel();

        //bool Need_Save=false;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            NoCard();

            this.MaxHeight = SystemParameters.WorkArea.Height + SystemParameters.BorderWidth * 2 + 100;
            this.MaxWidth = SystemParameters.WorkArea.Width + SystemParameters.BorderWidth * 2 + 100;

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

        void YesVisible()
        {
            MainCardsMenu.Visibility = Visibility.Visible;
            Veil.Visibility = Visibility.Hidden;
        }

        void NoVisible()
        {
            MainCardsMenu.Visibility = Visibility.Collapsed;
            Veil.Visibility = Visibility.Visible;
        }
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
            ExtendedCardsMenu.Visibility = Visibility.Visible;
            MainCardsMenu.Visibility = Visibility.Hidden;
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

        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Need_Save == true)
            {
                this.Show();
                if (MessageBox.Show("Точно хотите выйти? Все несохраненные данные будут удалены.",
                    "Выход",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else this.Close();
        }

        #endregion Window Manager Events
    }
}