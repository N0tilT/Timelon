using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TimelonCl.Data;

namespace TimelonCl
{
    /// <summary>
    /// Менеджер списков карт и провайдер данных
    /// Допустимо существование лишь одного экземпляра этого класса
    /// (зависимость от потока)
    /// </summary>
    public sealed class Manager
    {
        /// <summary>
        /// Название файла с данными
        /// </summary>
        public const string FileName = "data.xml";

        /// <summary>
        /// Название директории с данными
        /// </summary>
        public const string DirectoryName = "Timelon";

        /// <summary>
        /// Экземпляр класса одиночки
        /// </summary>
        private static Manager _instance = null;

        /// <summary>
        /// Глобальная точка доступа к классу
        /// Реализация шаблона Singleton
        /// </summary>
        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Manager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Полный путь до директории "Documents"
        /// </summary>
        private readonly string _source = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Списки карт
        /// </summary>
        private readonly SortedList<int, CardList> _list = new SortedList<int, CardList>();

        /// <summary>
        /// Закрепленные списки карт
        /// </summary>
        private readonly List<CardList> _listEssential = new List<CardList>
        {
            new CardList(0, "Задачи", true),
            new CardList(1, "Важное", true)
        };

        /// <summary>
        /// Конструктор менеджера
        /// Не может быть вызван извне
        /// </summary>
        private Manager()
        {
            Load();
        }

        /// <summary>
        /// Доступ к файлу с данными
        /// </summary>
        public string Source => Path.Combine(SourceDirectory, FileName);

        /// <summary>
        /// Доступ к директории с данными
        /// </summary>
        public string SourceDirectory => Path.Combine(_source, DirectoryName);

        /// <summary>
        /// Доступ к спискам карт
        /// </summary>
        public SortedList<int, CardList> All => _list;

        /// <summary>
        /// Получить список карт по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор списка карт</param>
        /// <returns>Список карт</returns>
        public CardList GetList(int id)
        {
            return All[id];
        }

        /// <summary>
        /// Вставить список карт
        /// </summary>
        /// <param name="list">Список карт</param>
        public void SetList(CardList list)
        {
            All[list.Id] = list;
        }

        /// <summary>
        /// Удалить список карт по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор списка карт</param>
        /// <returns>Статус успешности удаления</returns>
        public bool RemoveList(int id)
        {
            return All.Remove(id);
        }

        /// <summary>
        /// Проверить, существует ли список с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Статус проверки</returns>
        public bool ContainsList(int id)
        {
            return All.ContainsKey(id);
        }

        /// <summary>
        /// Поиск по части названия или описания по всем подспискам
        /// </summary>
        /// <param name="content">искомое значение</param>
        /// <returns>Список найденных карт</returns>
        public List<Card> SearchByContent(string content)
        {
            List<Card> result = new List<Card>();

            foreach (KeyValuePair<int, CardList> item in All)
            {
                foreach (Card card in item.Value.SearchByContent(content))
                {
                    result.Add(card);
                }
            }

            return result;
        }

        /// <summary>
        /// Внедрить закрепленные списки карт
        /// </summary>
        private void InjectEssentials()
        {
            foreach (CardList cardList in _listEssential)
            {
                if (ContainsList(cardList.Id))
                {
                    if (GetList(cardList.Id).Equals(cardList))
                    {
                        continue;
                    }
                }

                // Если под указанным идентификатором уже существует карта
                // Но ее название отличается от названия закрепленной
                // Это считается повреждением данных и карта будет перезаписана
                SetList(cardList);
            }
        }

        /// <summary>
        /// Сохранить данные в файл
        /// </summary>
        public void Sync()
        {
            Directory.CreateDirectory(SourceDirectory);

            if (!File.Exists(Source))
            {
                // Пустой файл
                File.Create(Source).Close();
            }

            // Внедряем закрепленные списки карт в начало
            InjectEssentials();

            List<CardListData> data = new List<CardListData>();

            foreach (KeyValuePair<int, CardList> item in All)
            {
                data.Add(item.Value.ToData());
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<CardListData>));

            // Даже если сохранять нечего
            // В любом случае мы создадим корректную xml основу
            // И пустой файл превратится в читаемый программой источник
            using (StreamWriter writer = new StreamWriter(Source))
            {
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Загрузить данные из файла
        /// </summary>
        private void Load()
        {
            Directory.CreateDirectory(SourceDirectory);

            if (!File.Exists(Source))
            {
                // Создадим пустой xml файл
                Sync();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<CardListData>));

            using (StreamReader reader = new StreamReader(Source))
            {
                // TODO: Бросает неприятные исключения при "повреждении" данных в файле
                List<CardListData> data = (List<CardListData>)serializer.Deserialize(reader);

                // Очищаем списки карт перед загрузкой новых
                All.Clear();

                foreach (CardListData item in data)
                {
                    SetList(CardList.FromData(item));
                }
            }

            // Внедряем закрепленные списки карт в конце
            // Тем самым ограничиваем возможность их случайной перезаписи
            InjectEssentials();
        }
    }
}