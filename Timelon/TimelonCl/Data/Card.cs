using System;

namespace TimelonCl.Data
{
    /// <summary>
    /// Контейнер данных дат для сериализации
    /// </summary>
    public class DateTimeContainerData : DataContainer
    {
        public DateTime Created;
        public DateTime? Updated;
        public DateTime? Planned;
    }

    /// <summary>
    /// Контейнер с датами
    /// </summary>
    public class DateTimeContainer
    {
        /// <summary>
        /// Создание контейнера по-умолчанию с текущей датой
        /// </summary>
        public static DateTimeContainer Now => new DateTimeContainer(DateTime.Now);

        /// <summary>
        /// Создать объект из контейнера с данными
        /// </summary>
        /// <param name="data">Контейнер с данными</param>
        /// <returns>Объект</returns>
        public static DateTimeContainer FromData(DateTimeContainerData data)
        {
            return new DateTimeContainer(data.Created, data.Updated, data.Planned);
        }

        /// <summary>
        /// Дата создания
        /// </summary>
        private DateTime _created;

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        private DateTime? _updated = null;

        /// <summary>
        /// Запланированная дата
        /// </summary>
        private DateTime? _planned = null;

        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="created">Дата создания</param>
        /// <param name="updated">Дата последнего обновления или null</param>
        /// <param name="planned">Запланированная дата или null</param>
        public DateTimeContainer(DateTime created, DateTime? updated = null, DateTime? planned = null)
        {
            Created = created;
            Updated = updated;
            Planned = planned;
        }

        /// <summary>
        /// Получить контейнер с данными из объекта
        /// </summary>
        /// <returns>Контейнер с данными</returns>
        public DateTimeContainerData ToData()
        {
            return new DateTimeContainerData
            {
                Created = Created,
                Updated = Updated,
                Planned = Planned
            };
        }

        /// <summary>
        /// Доступ к дате создания
        /// </summary>
        public DateTime Created
        {
            get => _created;
            private set
            {
                _created = value;

                // Перепроверяем зависимые даты
                // для предотвращения повреждения данных
                Updated = Updated;
                Planned = Planned;
            }
        }

        /// <summary>
        /// Доступ к дате последнего обновления
        /// </summary>
        public DateTime? Updated
        {
            get => _updated;
            set
            {
                if (value != null && value < Created)
                {
                    throw new ArgumentOutOfRangeException("updated не может быть раньше, чем created");
                }

                _updated = value;
            }
        }

        /// <summary>
        /// Доступ к запланированной дате
        /// </summary>
        public DateTime? Planned
        {
            get => _planned;
            set
            {
                if (value != null && value < Created)
                {
                    throw new ArgumentOutOfRangeException("planned не может быть раньше, чем created");
                }

                _planned = value;
            }
        }

        /// <summary>
        /// Проверить, задана ли дата последнего обновления
        /// </summary>
        /// <returns>Статус проверки</returns>
        public bool HasUpdated()
        {
            return Updated != null;
        }

        /// <summary>
        /// Проверить, задана ли запланированная дата
        /// </summary>
        /// <returns>Статус проверки</returns>
        public bool HasPlanned()
        {
            return Planned != null;
        }
    }

    /// <summary>
    /// Контейнер данных карты для сериализации
    /// </summary>
    public class CardData : UniqueData
    {
        public DateTimeContainerData Date;
        public string Description;
        public bool IsImportant;
        public bool IsCompleted;
    }

    /// <summary>
    /// Карта
    /// </summary>
    public class Card : Unique<Card>, IUniqueIdentifiable
    {
        /// <summary>
        /// Создать новую карту
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="created">Дата создания или null</param>
        /// <returns>Новая карта</returns>
        public static Card Make(string name, DateTime? created = null)
        {
            DateTimeContainer date = new DateTimeContainer(created ?? DateTime.Now);

            return new Card(UniqueId(), name, date);
        }

        /// <summary>
        /// Создать объект из контейнера с данными
        /// </summary>
        /// <param name="data">Контейнер с данными</param>
        /// <returns>Объект</returns>
        public static Card FromData(CardData data)
        {
            DateTimeContainer date = DateTimeContainer.FromData(data.Date);

            return new Card(data.Id, data.Name, date, data.Description, data.IsImportant, data.IsCompleted);
        }

        /// <summary>
        /// Контейнер с датами
        /// </summary>
        private DateTimeContainer _date;

        /// <summary>
        /// Описание
        /// </summary>
        private string _description = String.Empty;

        /// <summary>
        /// Статус важности
        /// </summary>
        private bool _isImportant = false;

        /// <summary>
        /// Статус выполнения
        /// </summary>
        private bool _isCompleted = false;

        /// <summary>
        /// Расширенный конструктор
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название</param>
        /// <param name="date">Контейнер с датами</param>
        /// <param name="description">Описание</param>
        /// <param name="isImportant">Статус важности</param>
        /// <param name="isCompleted">Статус выполнения</param>
        public Card(int id, string name, DateTimeContainer date, string description, bool isImportant, bool isCompleted) : this(id, name, date)
        {
            Description = description;
            IsImportant = isImportant;
            IsCompleted = isCompleted;
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название</param>
        /// <param name="date">Контейнер с датами</param>
        public Card(int id, string name, DateTimeContainer date) : base(id, name)
        {
            Date = date;
        }

        /// <summary>
        /// Получить контейнер с данными из объекта
        /// </summary>
        /// <returns>Контейнер с данными</returns>
        public CardData ToData()
        {
            return new CardData
            {
                Id = Id,
                Name = Name,
                Date = Date.ToData(),
                Description = Description,
                IsImportant = IsImportant,
                IsCompleted = IsCompleted
            };
        }

        /// <summary>
        /// Доступ к контейнеру с датами
        /// </summary>
        public DateTimeContainer Date
        {
            get => _date;
            private set => _date = value;
        }

        /// <summary>
        /// Доступ к описанию
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value.Trim();
        }

        /// <summary>
        /// Доступ к статусу важности
        /// </summary>
        public bool IsImportant
        {
            get => _isImportant;
            set => _isImportant = value;
        }

        /// <summary>
        /// Доступ к статусу выполнения
        /// </summary>
        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }

        public override string ToString()
        {
            return ToData().ToString();
        }
    }
}