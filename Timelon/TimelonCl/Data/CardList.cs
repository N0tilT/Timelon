using System;
using System.Collections.Generic;
using System.Linq;

namespace TimelonCl.Data
{
    /// <summary>
    /// Статус сортировки
    /// </summary>
    public enum SortOrder
    {
        // Данные обновлены или только что инициализированы
        // Необходимо провести сортировку
        Initial,

        // В произвольном формате (по возрастанию идентификаторов)
        Unsorted,

        // По возрастанию
        Ascending,

        // По убыванию
        Descending
    }

    /// <summary>
    /// Контейнер данных списка карт для сериализации
    /// </summary>
    public class CardListData : UniqueData
    {
        public bool IsEssential;
        public List<CardData> List;
    }

    /// <summary>
    /// Список карт
    /// </summary>
    public class CardList : Unique<CardList>, IUniqueIdentifiable
    {
        /// <summary>
        /// Создать новый список карт
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns>Новый список карт</returns>
        public static CardList Make(string name)
        {
            return new CardList(UniqueId(), name);
        }

        /// <summary>
        /// Создать объект из контейнера с данными
        /// </summary>
        /// <param name="data">Контейнер с данными</param>
        /// <returns>Объект</returns>
        public static CardList FromData(CardListData data)
        {
            List<Card> list = new List<Card>();

            foreach (CardData cardData in data.List)
            {
                list.Add(Card.FromData(cardData));
            }

            return new CardList(data.Id, data.Name, data.IsEssential, list);
        }

        /// <summary>
        /// Статус закрепления
        /// </summary>
        private bool _isEssential;

        /// <summary>
        /// Хранилище карт
        /// </summary>
        private readonly Dictionary<int, Card> _pool = new Dictionary<int, Card>();

        /// <summary>
        /// Отсортированный по дате обновления
        /// список идентификаторов карт
        /// (только невыполненные обычные)
        /// </summary>
        private readonly List<int> _idListDefault = new List<int>();

        /// <summary>
        /// Отсортированный по важности и дате обновления
        /// список идентификаторов карт
        /// (только невыполненные важные)
        /// </summary>
        private readonly List<int> _idListImportant = new List<int>();

        /// <summary>
        /// Отсортированный по статусу выполнения и дате обновления
        /// список идентификаторов карт
        /// (только выполненные)
        /// </summary>
        private readonly List<int> _idListCompleted = new List<int>();

        /// <summary>
        /// Статус сортировки
        /// </summary>
        private SortOrder _status = SortOrder.Initial;

        /// <summary>
        /// Конструктор списка из заданного списка карт
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="name">Название списка</param>
        /// <param name="isEssential">Статус закрепления</param>
        /// <param name="list">Список карт</param>
        public CardList(int id, string name, bool isEssential, List<Card> list) : this(id, name, isEssential)
        {
            foreach (Card card in list)
            {
                Set(card);
            }
        }

        /// <summary>
        /// Конструктор пустого списка
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <param name="name">Название списка</param>
        /// <param name="isEssential">Статус закрепления</param>
        /// <exception cref="ArgumentException"></exception>
        public CardList(int id, string name, bool isEssential = false) : base(id, name)
        {
            IsEssential = isEssential;
        }

        /// <summary>
        /// Получить контейнер с данными из объекта
        /// </summary>
        /// <returns>Контейнер с данными</returns>
        public CardListData ToData()
        {
            List<CardData> list = new List<CardData>();

            foreach (KeyValuePair<int, Card> item in All)
            {
                list.Add(item.Value.ToData());
            }

            return new CardListData
            {
                Id = Id,
                Name = Name,
                IsEssential = IsEssential,
                List = list
            };
        }

        /// <summary>
        /// Доступ к статусу закрепления
        /// </summary>
        public bool IsEssential
        {
            get => _isEssential;
            private set => _isEssential = value;
        }

        /// <summary>
        /// Доступ к хранилищу карт
        /// </summary>
        public Dictionary<int, Card> All => _pool;

        /// <summary>
        /// Доступ к статусу сортировки
        /// </summary>
        public SortOrder Status
        {
            get => _status;
            private set => _status = value;
        }

        /// <summary>
        /// Получить карту из хранилища по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор карты</param>
        /// <returns>Объект карты</returns>
        public Card Get(int id)
        {
            return All[id];
        }

        /// <summary>
        /// Получить отсортированный
        /// по дате обновления список карт
        /// (только невыполненные обычные)
        /// </summary>
        /// <param name="order">Статус сортировки</param>
        /// <returns>Список карт</returns>
        public List<Card> GetListDefault(SortOrder order = SortOrder.Descending)
        {
            Sort(order);

            List<Card> result = new List<Card>();

            foreach (int id in _idListDefault)
            {
                result.Add(Get(id));
            }

            return result;
        }

        /// <summary>
        /// Получить отсортированный по важности
        /// и дате обновления список карт
        /// (только невыполненные важные)
        /// </summary>
        /// <param name="order">Статус сортировки</param>
        /// <returns>Список карт</returns>
        public List<Card> GetListImportant(SortOrder order = SortOrder.Descending)
        {
            Sort(order);

            List<Card> result = new List<Card>();

            foreach (int id in _idListImportant)
            {
                result.Add(Get(id));
            }

            return result;
        }

        /// <summary>
        /// Получить отсортированный по статусу выполнения
        /// и дате обновления список карт
        /// (только выполненные)
        /// </summary>
        /// <param name="order">Статус сортировки</param>
        /// <returns>Список карт</returns>
        public List<Card> GetListCompleted(SortOrder order = SortOrder.Descending)
        {
            Sort(order);

            List<Card> result = new List<Card>();

            foreach (int id in _idListCompleted)
            {
                result.Add(Get(id));
            }

            return result;
        }

        /// <summary>
        /// Получить список карт по части названия или описания
        /// </summary>
        /// <param name="content">Часть названия или описания карты</param>
        /// <returns>Список карт</returns>
        public List<Card> SearchByContent(string content)
        {
            content = content.Trim().ToLower();

            List<Card> result = new List<Card>();

            foreach (KeyValuePair<int, Card> item in All)
            {
                if (!item.Value.Name.ToLower().Contains(content))
                {
                    continue;
                }

                if (!item.Value.Description.ToLower().Contains(content))
                {
                    continue;
                }

                result.Add(item.Value);
            }

            return result;
        }

        /// <summary>
        /// Получить список карт по дате обновления в заданном промежутке
        /// </summary>
        /// <param name="minDate">Начало</param>
        /// <param name="maxDate">Конец</param>
        /// <returns>Список карт</returns>
        public List<Card> SearchByDateUpdated(DateTime minDate, DateTime maxDate)
        {
            List<Card> result = new List<Card>();

            foreach (KeyValuePair<int, Card> item in All)
            {
                if (minDate > item.Value.Date.Updated)
                {
                    continue;
                }

                if (maxDate < item.Value.Date.Updated)
                {
                    continue;
                }

                result.Add(item.Value);
            }

            return result;
        }

        /// <summary>
        /// Сохранить карту
        /// </summary>
        /// <param name="card">Объект карты</param>
        public void Set(Card card)
        {
            Status = SortOrder.Initial;
            All[card.Id] = card;
        }

        /// <summary>
        /// Удалить карту с заданным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор карты</param>
        /// <returns>Статус успеха удаления</returns>
        public bool Remove(int id)
        {
            Status = SortOrder.Initial;

            return All.Remove(id);
        }

        /// <summary>
        /// Проверить, существует ли карта с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Статус проверки</returns>
        public bool Contains(int id)
        {
            return All.ContainsKey(id);
        }

        /// <summary>
        /// Выбрать подходящий источник для указанного направления сортировки
        /// </summary>
        /// <param name="order">Статус сортировки</param>
        /// <returns>Источник</returns>
        private IEnumerable<KeyValuePair<int, Card>> Source(SortOrder order)
        {
            if (order == SortOrder.Unsorted)
            {
                return All;
            }

            if (order == SortOrder.Ascending)
            {
                return All.OrderBy(item => item.Value.Date.Updated ?? item.Value.Date.Created);
            }

            return All.OrderByDescending(item => item.Value.Date.Updated ?? item.Value.Date.Created);
        }

        /// <summary>
        /// Определить идентификатор карты в один из списков для сортировки
        /// </summary>
        /// <param name="card">Карта</param>
        private void Cache(Card card)
        {
            if (card.IsCompleted == true)
            {
                _idListCompleted.Add(card.Id);
                return;
            }

            if (card.IsImportant == true)
            {
                _idListImportant.Add(card.Id);
                return;
            }

            _idListDefault.Add(card.Id);
        }

        /// <summary>
        /// Отсортировать списки идентификаторов
        /// TODO: Оценить скорость
        /// </summary>
        /// <param name="order">Статус сортировки</param>
        private void Sort(SortOrder order)
        {
            if (order == Status)
            {
                // Списки уже отсортированы в указанном формате
                return;
            }

            _idListDefault.Clear();
            _idListImportant.Clear();
            _idListCompleted.Clear();

            foreach (KeyValuePair<int, Card> item in Source(order))
            {
                Cache(item.Value);
            }

            Status = order;
        }

        public bool Equals(CardList list)
        {
            return base.Equals(list) && list.IsEssential == IsEssential;
        }

        public override string ToString()
        {
            return ToData().ToString();
        }
    }
}