using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timelon.Data;

namespace Timelon.Test
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void TestGetSetRemoveContainsList()
        {
            // TODO: Надо обдумать как протестить все не сломать ничего
            // Запись и чтение данных пока скипаем
            // Приватные функции через PrivateObject
            // Лучше всего сбацать сюда конструктор и подготовить менеджер
            // на этапе инициализации
            Manager manager = Manager.Instance;
            CardList list = new CardList(0, "SAMPLE");

            manager.SetList(list);

            Assert.IsTrue(ReferenceEquals(manager.GetList(0), list));
            Assert.IsTrue(manager.ContainsList(0));
            Assert.IsTrue(manager.RemoveList(0));
        }

        // TODO: Протестировать менеджер и все остальное
    }
}
