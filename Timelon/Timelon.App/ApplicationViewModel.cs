using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Timelon.App.Core;
using Timelon.Data;
using System.Windows.Data;
using System.Windows;
using System;

namespace Timelon.App
{
    public class ExtendedCard : Card
    {
        private int _parentListId;
        private string _parentListName;

        public int ParentId
        {
            get { return _parentListId; }
            set { _parentListId = value; }
        }

        public string ParentName
        {
            get { return _parentListName; }
            set { _parentListName = value; }
        }
        
        public ExtendedCard(int listId,
            string listName, int cardId,
            string cardName, DateTimeContainer date,
            string description, bool isImportant, bool isCompleted)
            :
            base(cardId, cardName, date, description, isImportant, isCompleted)
        {
            _parentListId = listId;
            _parentListName = listName;
        }
    }

    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Выбранная карта
        /// </summary>
        private Card _selectedCard;

        /// <summary>
        /// Выбранный список карт
        /// </summary>
        private CardList _selectedList;

        /// <summary>
        /// Текущий менеджер списков
        /// </summary>
        public Manager _listManager;

        /// <summary>
        /// Выбранная расширенная карта
        /// </summary>
        private ExtendedCard _selectedExtendedCard;

        /// <summary>
        /// Список расширенных карт
        /// </summary>
        public List<ExtendedCard> _extendedCardList;

        /// <summary>
        /// Коллекция карт - результата работы поиска
        /// </summary>
        private ObservableCollection<ExtendedCard> _extendedCards = new ObservableCollection<ExtendedCard>();

        /// <summary>
        /// Коллекция списков
        /// </summary>
        private ObservableCollection<CardList> _lists = new ObservableCollection<CardList>();

        /// <summary>
        /// Коллекция важных карт, отсортированных по дате
        /// </summary>
        private ObservableCollection<Card> _importantCards = new ObservableCollection<Card>();

        /// <summary>
        /// Коллекция невыполненных карт, отсортированных по дате
        /// </summary>
        private ObservableCollection<Card> _defaultCards = new ObservableCollection<Card>();

        /// <summary>
        /// Коллекция выполненных карт, отсортированных по дате
        /// </summary>
        private ObservableCollection<Card> _doneCards = new ObservableCollection<Card>();

        /// <summary>
        /// Индикатор необходимости сохранения
        /// </summary>
        public bool Need_Save = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Доступ к коллекции списков
        /// </summary>
        public ObservableCollection<CardList> Lists
        { get { return _lists; } set { _lists = value; OnPropertyChanged("Lists"); } }

        /// <summary>
        /// Доступ к менеджеру списков
        /// </summary>
        public Manager ListManager
        { get { return _listManager; } set { _listManager = value; } }

        /// <summary>
        /// Доступ к важным задачам
        /// </summary>
        public ObservableCollection<Card> ImportantCards
        {
            get
            {
                return _importantCards;
            }
            set
            {
                _importantCards = value;


                OnPropertyChanged("ImportantCards");
            }
        }

        /// <summary>
        /// Доступ к невыполненным задачам
        /// </summary>
        public ObservableCollection<Card> DefaultCards
        {
            get
            {
                return _defaultCards;
            }
            set
            {
                _defaultCards = value;

                OnPropertyChanged("DefaultCards");
            }
        }

        /// <summary>
        /// Доступ к выполненным задачам
        /// </summary>
        public ObservableCollection<Card> DoneCards
        {
            get
            {
                return _doneCards;
            }
            set
            {
                _doneCards = value;


                OnPropertyChanged("DoneCards");
            }
        }

        /// <summary>
        /// Доступ к выбранной карте
        /// </summary>
        public Card SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                OnPropertyChanged("SelectedCard");
            }
        }

        /// <summary>
        /// Доступ к выбранному списку
        /// </summary>
        public CardList SelectedList
        {
            get { return _selectedList; }
            set
            {
                _selectedList = value;

                //Перезаполняем коллекции карт выбранного списка
                UpdateCardsCollections();

                OnPropertyChanged("SelectedList");
            }
        }

        /// <summary>
        /// Доступ к выбранной расширенной карте
        /// </summary>
        public ExtendedCard SelectedExtendedCard
        {
            get { return _selectedExtendedCard; }
            set
            {
                _selectedExtendedCard = value;
                OnPropertyChanged("SelectedExtendedCard");
            }
        }

        /// <summary>
        /// Доступ к списку расширенных задач
        /// </summary>
        public ObservableCollection<ExtendedCard> ExtendedCards
        {
            get
            {
                return _extendedCards;
            }
            set
            {
                _extendedCards = value;
                OnPropertyChanged("ExtendedCards");
            }
        }


        #endregion Properties

        #region Commands

        /// <summary>
        /// Команда отметки задачи(карточки) как важной
        /// </summary>
        private RelayCommand cardImportantCommand;

        /// <summary>
        /// Отметить задачу как важную
        /// </summary>
        public RelayCommand CardImportantCommand => cardImportantCommand ??
                    (cardImportantCommand = new RelayCommand(obj =>
                    {
                        if (obj is Card iCard)
                        {
                            if (!iCard.IsImportant)
                            {
                                iCard.IsImportant = true;
                                SelectedList.Set(iCard);
                                UpdateCardsCollections();
                                SelectedCard = SelectedList.Get(iCard.Id);
                            }
                        }
                    }));

        /// <summary>
        /// Команда отметки задачи(карточки) как не важной
        /// </summary>
        private RelayCommand cardUndoImportantCommand;

        /// <summary>
        /// Отменить отметку о важности
        /// </summary>
        public RelayCommand CardUndoImportantCommand => cardUndoImportantCommand ??
                    (cardUndoImportantCommand = new RelayCommand(obj =>
                    {
                        if (obj is Card unimpCard)
                        {
                            if (unimpCard.IsImportant)
                            {
                                unimpCard.IsImportant = false;
                                SelectedList.Set(unimpCard);

                                UpdateCardsCollections();
                                SelectedCard = SelectedList.Get(unimpCard.Id);
                            }
                        }
                    }));

        /// <summary>
        /// Команда завершения задачи(карточки)
        /// </summary>
        private RelayCommand cardDoneCommand;

        /// <summary>
        /// Выполнить задачу
        /// </summary>
        public RelayCommand CardDoneCommand => cardDoneCommand ??
                    (cardDoneCommand = new RelayCommand(obj =>
                    {
                        if (obj is Card cCard)
                        {
                            if (!cCard.IsCompleted)
                            {
                                cCard.IsCompleted = true;
                                SelectedList.Set(cCard);    //Обновляем карту в списке

                                UpdateCardsCollections();
                                SelectedCard = SelectedList.Get(cCard.Id);
                            }
                        }
                    }));

        /// <summary>
        /// Команда восстановления задачи(карточки) из выполненных в невыполненные
        /// </summary>
        private RelayCommand cardRecoverCommand;

        /// <summary>
        /// Восстановить задачу(карту) в невыполненные
        /// </summary>
        public RelayCommand CardRecoverCommand => cardRecoverCommand ??
                    (cardRecoverCommand = new RelayCommand(obj =>
                    {
                        if (obj is Card recCard)
                        {
                            if (recCard.IsCompleted)
                            {
                                recCard.IsCompleted = false;
                                SelectedList.Set(recCard);
                                UpdateCardsCollections();
                                SelectedCard = SelectedList.Get(recCard.Id);
                            }
                        }
                    }));

        /// <summary>
        /// Команда удаления карты из списка
        /// </summary>
        private RelayCommand removeCardCommand;

        /// <summary>
        /// Удаление карты из списка
        /// </summary>
        public RelayCommand RemoveCardCommand => removeCardCommand ??
                    (removeCardCommand = new RelayCommand(obj =>
                    {
                        if (obj is Card rCard)
                        {
                            if (rCard.IsCompleted)
                            {
                                DoneCards.Remove(rCard);
                                SelectedList.Remove(rCard.Id);
                            }
                            else if (rCard.IsImportant)
                            {
                                ImportantCards.Remove(rCard);
                                SelectedList.Remove(rCard.Id);
                            }
                            else
                            {
                                DefaultCards.Remove(rCard);
                                SelectedList.Remove(rCard.Id);
                            }
                        }
                    },
                    (obj) => DefaultCards.Count > 0 || DoneCards.Count > 0 || ImportantCards.Count > 0));  //Удаляем карты, только если они есть в списке

        /// <summary>
        /// Команда удаления списка
        /// </summary>
        private RelayCommand removeListCommand;

        public RelayCommand RemoveListCommand => removeListCommand ??
            (removeListCommand = new RelayCommand(obj =>
            {
                if (obj is CardList rList)
                {
                    if (!rList.IsEssential)
                    {
                        SelectedList = ListManager.All[0];
                        ListManager.RemoveList(rList.Id);
                        Lists.Remove(rList);
                    }
                }
            }));

        /// <summary>
        /// Команда добавления нового списка
        /// </summary>
        private RelayCommand addListCommand;

        /// <summary>
        /// Добавить новый список
        /// </summary>
        public RelayCommand AddListCommand => addListCommand ??
                    (addListCommand = new RelayCommand(obj =>
                    {
                        TextBox tmp = obj as TextBox;
                        if (tmp.Text != "")
                        {
                            CardList newList = new CardList(tmp.Text);
                            ListManager.SetList(newList);
                            Lists.Add(newList);
                            SelectedList = newList;
                        }
                    }));

        /// <summary>
        /// Команда добавления новой карты
        /// </summary>
        private RelayCommand addCardCommand;

        /// <summary>
        /// Добавить новую карту
        /// </summary>
        public RelayCommand AddCardCommand => addCardCommand ??
                    (addCardCommand = new RelayCommand(obj =>
                    {
                        TextBox tmp = obj as TextBox;
                        if (tmp.Text != "")
                        {
                            Card newCard = new Card(tmp.Text);
                            SelectedList.Set(newCard);
                            DefaultCards.Add(newCard);
                            SelectedCard = newCard;
                        }
                    }));

        /// <summary>
        /// Команда поиска карты по содержимому
        /// </summary>
        private RelayCommand searchCardCommand;

        /// <summary>
        /// Поиск карты по содержимому
        /// </summary>
        public RelayCommand SearchCardCommand => searchCardCommand ??
                    (searchCardCommand = new RelayCommand(obj =>
                    {
                        TextBox tmp = obj as TextBox;
                        if (tmp.Text != "")
                        {
                            _extendedCardList = new List<ExtendedCard>();
                            foreach (KeyValuePair<int, CardList> item in _listManager.All)
                            {
                                List<Card> searchResult = item.Value.SearchByContent(tmp.Text);
                                foreach (Card card in searchResult)
                                    _extendedCardList.Add(new ExtendedCard(item.Value.Id,
                                        item.Value.Name, card.Id, card.Name,
                                        card.Date, card.Description, card.IsImportant, card.IsCompleted));
                            }

                            ExtendedCards = new ObservableCollection<ExtendedCard>(_extendedCardList);
                        }
                    }));

        /// <summary>
        /// Команда отображения списка-родителя карты
        /// </summary>
        private RelayCommand showParentListCommand;

        public RelayCommand ShowParentListCommand => showParentListCommand ??
            (showParentListCommand = new RelayCommand(obj =>
            {
                if (obj is ExtendedCard rCard)
                {
                    SelectedList = ListManager.GetList(rCard.ParentId);
                    SelectedCard = SelectedList.Get(rCard.Id);
                    ExtendedCards = null;
                }
            }));

        #endregion Commands

        #region Methods

        /// <summary>
        /// Метод обновления коллекций отображаемых карт
        /// </summary>
        private void UpdateCardsCollections()
        {
            ImportantCards = new ObservableCollection<Card>(_selectedList.GetListImportant(SortOrder.Ascending));
            DefaultCards = new ObservableCollection<Card>(_selectedList.GetListDefault(SortOrder.Ascending));
            DoneCards = new ObservableCollection<Card>(_selectedList.GetListCompleted(SortOrder.Initial));
            SelectedCard = _selectedCard;
        }

        #endregion Methods

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ApplicationViewModel()
        {
            //Загрузка данных из файла .xml
            ListManager = Manager.Instance;

            //Изначально выбран первый список - по умолчанию "Задачи"
            SelectedList = ListManager.All[0];


            //Загрузка списков в коллекцию
            Lists = new ObservableCollection<CardList>(ListManager.All.Values);

            UpdateCardsCollections();
        }

        /// <summary>
        /// Определение интерфейса событий
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}