﻿using System;

namespace Timelon.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация менеджера как можно раньше
            _ = Manager.Instance;
            ConsoleTest test = new ConsoleTest();

            // Запуск цепочки тестирования в консоли
            test.TestRandomCard(3);
            test.TestCardList(20);
            test.TestCardListManager(3, 5);
            test.MeasureCardListOperationsTime(100000);

            Console.ReadKey();
        }
    }
}