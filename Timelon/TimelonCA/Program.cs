using System;
using TimelonCl;

namespace TimelonCA
{
    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация менеджера как можно раньше
            Manager manager = Manager.Instance;
            ConsoleTest test = new ConsoleTest();

            // Запуск цепочки тестирования в консоли
            test.TestRandomCard();
            test.TestCardList();
            test.TestCardListManager();
            test.MeasureCardListOperationsTime();

            Console.ReadKey();
        }
    }
}
