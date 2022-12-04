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
        /// <param name="cardCount">Количество карт</param>
        public void TestRandomCard(int cardCount)
        {
            Console.WriteLine("Создание случайных карт:");

            for (int i = 0; i < cardCount; i++)
            {
                Console.WriteLine(Randomizer.RandomCard());
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Запустить тест создания случайного списка карт и сортировки
        /// </summary>
        /// <param name="cardCount">Количество карт в списке</param>
        public void TestCardList(int cardCount)
        {
            Console.WriteLine("Создание случайного списка карт и сортировки:");

            CardList list = Randomizer.RandomCardList(cardCount);

            Console.WriteLine();
            Console.WriteLine("В произвольном порядке:");

            foreach (Card card in list.GetListDefault(SortOrder.Unsorted))
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
            Console.WriteLine("Важные по дате обновления по возрастанию:");

            foreach (Card card in list.GetListImportant(SortOrder.Ascending))
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
            Console.WriteLine("Выполненные по дате обновления по убыванию:");

            foreach (Card card in list.GetListCompleted(SortOrder.Descending))
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Запустить тест создания случайных списков карт в менеджере и работы с данными
        /// </summary>
        /// <param name="cardlistCount">Количество списков</param>
        /// <param name="cardCount">Количество карт в списке</param>
        public void TestCardListManager(int cardlistCount, int cardCount)
        {
            Console.WriteLine("Создание случайных списков карт в менеджере и работа с данными:");

            Manager manager = Manager.Instance;

            // Данные загружаются из файла
            // Так что мы их перезапишем
            // Но идентификаторы продолжат инкременироваться (это ок)
            manager.All.Clear();

            for (int i = 0; i < cardlistCount; i++)
            {
                manager.SetList(Randomizer.RandomCardList(cardCount));
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
        /// <param name="cardCount">Количество карт в списке</param>
        public void MeasureCardListOperationsTime(int cardCount)
        {
            Console.WriteLine("Замер времени выполнения основных операций списка карт:");

            Stopwatch watch = new Stopwatch();

            watch.Start();
            CardList list = Randomizer.RandomCardList(cardCount);
            watch.Stop();

            Console.WriteLine($"Создание случайного списка карт ({cardCount}): {watch.Elapsed}");

            watch.Restart();
            list.GetListDefault(SortOrder.Unsorted);
            watch.Stop();

            Console.WriteLine($"Сортировка в произвольном порядке ({cardCount}): {watch.Elapsed}");

            watch.Restart();
            list.GetListDefault(SortOrder.Ascending);
            watch.Stop();

            Console.WriteLine($"Сортировка по возрастанию ({cardCount}): {watch.Elapsed}");

            watch.Restart();
            list.GetListDefault(SortOrder.Descending);
            watch.Stop();

            Console.WriteLine($"Сортировка по убыванию ({cardCount}): {watch.Elapsed}");

            watch.Restart();
            list.SearchByContent(Randomizer.Random.NextString(4, 8));
            watch.Stop();

            Console.WriteLine($"Поиск по части названия или описания карты ({cardCount}): {watch.Elapsed}");

            DateTime date = Randomizer.Random.NextDateTime();

            watch.Restart();
            list.SearchByDateUpdated(date, date.AddDays(120));
            watch.Stop();

            Console.WriteLine($"Поиск по дате обновления карты ({cardCount}): {watch.Elapsed}");
        }
    }
}
