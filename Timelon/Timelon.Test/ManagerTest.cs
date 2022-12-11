using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private const string Source = "sample";

        /// <summary>
        /// Экземпляр менеджера
        /// </summary>
        private Manager _manager;

        /// <summary>
        /// Экземпляр открытого менеджера для отладки
        /// </summary>
        private PrivateObject _managerAccessor;

        /// <summary>
        /// Экземпляры списков карт
        /// </summary>
        private readonly List<CardList> _sampleList = new List<CardList>()
        {
            new CardList(10, "SampleX"),
            new CardList(11, "SampleY")
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        public ManagerTest()
        {
            ResetManager();
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
            string expected = Path.Combine(Source, Manager.DirectoryName);
            string actual = _manager.SourceDirectory.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Тест доступа к полю Source
        /// </summary>
        [TestMethod]
        public void TestSourceProperty()
        {
            string expected = Path.Combine(Source, Manager.DirectoryName);
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
            CardList value = _manager.GetList(_sampleList[0].Id);

            Assert.IsNotNull(value);
        }

        /// <summary>
        /// Тест метода SetList и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestSetList()
        {
            int notExpected = _manager.All.Count;
            int actual = _manager.All.Count + 1;

            _manager.SetList(_sampleList[1]);
            ResetManager();

            Assert.AreNotEqual(notExpected, actual);
        }

        /// <summary>
        /// Тест метода RemoveList и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestRemoveList()
        {
            int notExpected = _manager.All.Count;
            int actual = _manager.All.Count - 1;

            _manager.RemoveList(_sampleList[0].Id);
            ResetManager();

            Assert.AreNotEqual(notExpected, actual);
        }

        /// <summary>
        /// Тест метода ContainsList
        /// </summary>
        [TestMethod]
        public void TestContainsList()
        {
            bool condition = _manager.ContainsList(_sampleList[0].Id);

            Assert.IsTrue(condition);
        }

        /// <summary>
        /// Тест методов SaveData и LoadData и сброс изменений
        /// </summary>
        [TestMethod]
        public void TestSaveLoadData()
        {
            SortedList<int, CardList> expected = _manager.All;

            _manager.SaveData();
            ResetManager();
            _managerAccessor.Invoke("LoadData");

            SortedList<int, CardList> actual = _manager.All;

            Assert.AreEqual(expected.Count, actual.Count);

            // TODO: Проверять точнее?
            // for (int i = 0; i < expected.Count; i++)
            // {
            //     Assert.IsTrue(actual[i].Equals(expected[i]));
            // }

            ResetManager();
        }

        /// <summary>
        /// Тест создания файла с данными для отладки
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
            _manager = new Manager(Source);
            _managerAccessor = new PrivateObject(_manager);

            // На данном этапе необходим только один список карт
            _manager.SetList(_sampleList[0]);
        }
    }
}
