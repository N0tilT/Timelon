using System;
using System.Collections.Generic;
using System.Diagnostics;
using TimelonCl;
using TimelonCl.Data;

namespace TimelonCA
{
    /// <summary>
    /// Класс консольного тестирования
    /// </summary>
    class ConsoleTest
    {
        /// <summary>
        /// Запустить тест создания случайных карт
        /// </summary>
        public void TestRandomCard()
        {
            Console.WriteLine("TestRandomCard:");

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(Randomizer.RandomCard());
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Запустить тест создания случайного списка карт и сортировки
        /// </summary>
        public void TestCardList()
        {
            Console.WriteLine("TestCardList:");

            CardList list = Randomizer.RandomCardList(20);

            Console.WriteLine();
            Console.WriteLine("DEFAULT LIST UNSORTED:");

            // В произвольном порядке
            foreach (Card card in list.GetListDefault(SortOrder.Unsorted))
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
            Console.WriteLine("IMPORTANT ASCENDING:");

            // Сортировка важных по дате обновления по возрастанию
            foreach (Card card in list.GetListImportant(SortOrder.Ascending))
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
            Console.WriteLine("COMPLETED DESCENDING:");

            // Сортировка выполненных по дате обновления по убыванию
            foreach (Card card in list.GetListCompleted(SortOrder.Descending))
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Запустить тест создания случайных списков карт в менеджере и работы с данными
        /// </summary>
        public void TestCardListManager()
        {
            Console.WriteLine("TestCardListManager:");

            Manager manager = Manager.Instance;

            // Данные загружаются из файла
            // Так что мы их перезапишем
            // Но идентификаторы продолжат инкременироваться (это ок)
            manager.All.Clear();

            for (int i = 0; i < 3; i++)
            {
                manager.SetList(Randomizer.RandomCardList(5));
            }

            manager.Sync();

            foreach (KeyValuePair<int, CardList> item in manager.All)
            {
                Console.WriteLine(item.Value);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Замерить время выполнения основных операций списка карт
        /// </summary>
        public void MeasureCardListOperationsTime()
        {
            Console.WriteLine("MeasureCardListOperations:");

            Stopwatch watch = new Stopwatch();

            watch.Start();
            CardList list = Randomizer.RandomCardList(100000);
            watch.Stop();

            Console.WriteLine($"Создание случайного списка карт (100000): {watch.Elapsed}");

            watch.Restart();
            list.GetListDefault(SortOrder.Unsorted);
            watch.Stop();

            Console.WriteLine($"Сортировка в произвольном порядке (100000): {watch.Elapsed}");

            watch.Restart();
            list.GetListDefault(SortOrder.Ascending);
            watch.Stop();

            Console.WriteLine($"Сортировка по возрастанию (100000): {watch.Elapsed}");

            watch.Restart();
            list.GetListDefault(SortOrder.Descending);
            watch.Stop();

            Console.WriteLine($"Сортировка по убыванию (100000): {watch.Elapsed}");

            watch.Restart();
            list.SearchByContent(Randomizer.Random.NextString(4, 8));
            watch.Stop();

            Console.WriteLine($"Поиск по части названия или описания карты (100000): {watch.Elapsed}");

            DateTime date = Randomizer.Random.NextDateTime();

            watch.Restart();
            list.SearchByDateUpdated(date, date.AddDays(120));
            watch.Stop();

            Console.WriteLine($"Поиск по дате обновления карты (100000): {watch.Elapsed}");
        }
    }
}
