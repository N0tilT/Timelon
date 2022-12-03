using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace TimelonCl.Data
{
    /// <summary>
    /// Интерфейс, обозначающий принадлежность к идентифицируемому виду
    /// </summary>
    public interface IUniqueIdentifiable
    {
        //
    }

    /// <summary>
    /// Базовый контейнер данных для сериализации
    /// </summary>
    public abstract class DataContainer
    {
        /// <summary>
        /// Получить подробное представление объекта в виде строки
        /// Используется формат XML
        /// </summary>
        /// <returns>Строка с данными объекта</returns>
        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(GetType());

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);

                return writer.ToString();
            }
        }
    }

    /// <summary>
    /// Контейнер данных уникального класса для сериализации
    /// </summary>
    public abstract class UniqueData : DataContainer
    {
        [XmlAttribute]
        public int Id;

        public string Name;
    }

    /// <summary>
    /// Абстрактный класс для хранения и генерации уникальных идентификаторов
    /// </summary>
    public abstract class Unique<IUniqueIdentifiable>
    {
        /// <summary>
        /// Счетчик
        /// </summary>
        private static int _incrementor = 0;

        /// <summary>
        /// Зарегистрировать уникальный идентификатор в текущей сессии
        /// </summary>
        /// <param name="id">Идентификатор</param>
        protected static void Register(int id)
        {
            if (id <= _incrementor)
            {
                return;
            }

            Interlocked.Exchange(ref _incrementor, id + 1);
        }

        /// <summary>
        /// Получить следующий уникальный идентификатор
        /// </summary>
        /// <returns>Уникальный идентификатор</returns>
        protected static int UniqueId()
        {
            int result = _incrementor;

            Interlocked.Increment(ref _incrementor);
            return result;
        }

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private int _id;

        /// <summary>
        /// Название
        /// </summary>
        private string _name;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название</param>
        public Unique(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Доступ к уникальному идентификатору
        /// </summary>
        public int Id
        {
            get => _id;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("id не может быть отрицательным числом");
                }

                _id = value;

                // Регистрируем идентификатор в текущей сессии
                // во избежание дублирования
                Register(value);
            }
        }

        /// <summary>
        /// Доступ к названию
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("name не может быть пустой строкой");
                }

                _name = value.Trim();
            }
        }

        public bool Equals(Unique<IUniqueIdentifiable> unique)
        {
            return unique.Id == Id && unique.Name == Name;
        }
    }
}