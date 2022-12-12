using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Timelon.Data;

namespace Timelon.Test
{
    [TestClass]
    public class ManagerTest
    {
        /// <summary>
        /// Путь до директории с файлом с данными для отладки
        /// /bin/DEBUG/
        /// </summary>
        private const string SampleSource = "sample";

        /// <summary>
        /// Зарезервированный идентификатор списка карт A
        /// </summary>
        private const int ReservedIdA = Manager.EssentialIdB + 1;

        /// <summary>
        /// Зарезервированный идентификатор списка карт B
        /// </summary>
        private const int ReservedIdB = ReservedIdA + 1;

        /// <summary>
        /// Зарезервированный идентификатор несуществующего списка карт
        /// </summary>
        private const int ReservedBadId = ReservedIdB + 1;

        /// <summary>
        /// Экземпляр менеджера
        /// </summary>
        private Manager _manager;

        /// <summary>
        /// Экземпляр открытого менеджера для отладки
        /// </summary>
        private PrivateObject _managerAccessor;

        /// <summary>
        /// Список карт A
        /// </summary>
        private readonly CardList _cardListA = new CardList(ReservedIdA, "SampleA");

        /// <summary>
        /// Список карт B
        /// </summary>
        private readonly CardList _cardListB = new CardList(ReservedIdB, "SampleB");

        /// <summary>
        /// Конструктор
        /// </summary>
        public ManagerTest()
        {
            ResetManager();
        }

        /// <summary>
        /// Тест конструктора
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Manager value = new Manager(SampleSource);
            string badSource = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Assert.IsNotNull(value);
            Assert.ThrowsException<ArgumentException>(() => new Manager(badSource));
        }

        /// <summary>
        /// Тест доступа к полю Instance
        /// </summary>
        [TestMethod]
        public void TestInstanceProperty()
        {
            Manager value = Manager.Instance;

            Assert.IsInstanceOfType(value, typeof(Manager));
        }

        /// <summary>
        /// Тест доступа к полю SourceDirectory
        /// </summary>
        [TestMethod]
        public void TestSourceDirectoryProperty()
        {
            string expected = Path.Combine(SampleSource, Manager.DirectoryName);
            string actual = _manager.SourceDirectory.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Тест доступа к полю Source
        /// </summary>
        [TestMethod]
        public void TestSourceProperty()
        {
            string expected = Path.Combine(SampleSource, Manager.DirectoryName);
            expected = Path.Combine(expected, Manager.FileName);

            string actual = _manager.Source.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Тест доступа к статическому полю Instance
        /// </summary>
        [TestMethod]
        public void TestAllProperty()
        {
            SortedList<int, CardList> value = _manager.All;

            Assert.IsInstanceOfType(value, typeof(SortedList<int, CardList>));
        }

        /// <summary>
        /// Тест метода GetList
        /// </summary>
        [TestMethod]
        public void TestGetList()
        {
            CardList value = _manager.GetList(_cardListA.Id);

            Assert.IsNotNull(value);
            Assert.ThrowsException<KeyNotFoundException>(() => _manager.GetList(ReservedBadId));
        }

        /// <summary>
        /// Тест метода SetList и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestSetList()
        {
            int notExpected = _manager.All.Count;

            // Список карт A уже находится в менеджере
            _manager.SetList(_cardListA);
            _manager.SetList(_cardListB);

            int actual = _manager.All.Count;

            Assert.IsTrue(actual - notExpected == 1);
            ResetManager();
        }

        /// <summary>
        /// Тест метода RemoveList и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestRemoveList()
        {
            int notExpected = _manager.All.Count;

            // Список карт A есть в менеджере
            _manager.RemoveList(_cardListA.Id);

            Assert.AreNotEqual(notExpected, _manager.All.Count);
            ResetManager();
        }

        /// <summary>
        /// Тест метода RemoveList с отсутствующим списком карт и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestRemoveNotExistingList()
        {
            int expected = _manager.All.Count;

            // Список карт B отсутствует в менеджере
            _manager.RemoveList(_cardListB.Id);

            Assert.AreEqual(expected, _manager.All.Count);
            ResetManager();
        }

        /// <summary>
        /// Тест метода ContainsList
        /// </summary>
        [TestMethod]
        public void TestContainsList()
        {
            bool conditionGood = _manager.ContainsList(_cardListA.Id);
            bool conditionBad = _manager.ContainsList(_cardListB.Id);

            Assert.IsTrue(conditionGood);
            Assert.IsFalse(conditionBad);
        }

        /// <summary>
        /// Тест метода SaveData включая всю последовательность работы с данными и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestSaveLoadDataSequence()
        {
            int expected = _manager.All.Count;

            _manager.SetList(_cardListB);
            _manager.SaveData();

            ResetManager();
            _managerAccessor.Invoke("LoadData");

            int actual = _manager.All.Count;

            Assert.AreNotEqual(expected, actual);

            ResetManager();
        }

        /// <summary>
        /// Тест метода CreateDataSource
        /// </summary>
        [TestMethod]
        public void TestCreateDataSource()
        {
            _managerAccessor.Invoke("CreateDataSource");

            string source = _managerAccessor.GetProperty("Source").ToString();
            bool condition = File.Exists(source);

            Assert.IsTrue(condition);
        }

        /// <summary>
        /// Тест метода InjectEssentials и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestInjectEssentials()
        {
            _managerAccessor.Invoke("InjectEssentials");

            List<CardList> essentials = (List<CardList>)_managerAccessor.GetField("_listEssential");

            foreach (CardList item in essentials)
            {
                Assert.IsTrue(_manager.ContainsList(item.Id));
            }

            ResetManager();
        }

        /// <summary>
        /// Вернуть менеджер к начальному состоянию
        /// </summary>
        private void ResetManager()
        {
            _manager = new Manager(SampleSource);
            _managerAccessor = new PrivateObject(_manager);

            // На этапе инициализации необходим только один список карт
            _manager.SetList(_cardListA);
        }
    }
}
