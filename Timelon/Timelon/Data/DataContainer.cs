using System.IO;
using System.Xml.Serialization;

namespace Timelon.Data
{
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
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(GetType());

                serializer.Serialize(writer, this);

                return writer.ToString();
            }
        }
    }
}