using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Timelon.Data;

namespace Timelon.Test.Data
{
    /// <summary>
    /// Модульное тестирование класса CardList
    /// </summary>
    [TestClass]
    public class CardListTest
    {
        public class TestCardList : CardList
        {
            public TestCardList(int id, string name, bool isEssential, List<Card> list) : base(id, name, isEssential, list)
            {
            }
        }

        private TestCardList listA = new TestCardList(0, "EssentialTestListA", true, new List<Card> 
        {
            new Card(0,"CardA",DateTimeContainer.Now,"This is CardA", true,true),
            new Card(1,"CardB",DateTimeContainer.Now,"This is CardB", true,false),
            new Card(2,"CardC",DateTimeContainer.Now,"This is CardC", false,true),
            new Card(3,"CardD",DateTimeContainer.Now,"This is CardD", false,false),
            new Card(4,"CardE",DateTimeContainer.Now,"This is CardE", false,false),
            new Card(5,"CardF",DateTimeContainer.Now,"This is CardF", true,false),
        });
        private TestCardList listB = new TestCardList(1, "TestListB", false, new List<Card>
        {
            new Card("CardA"),
            new Card("CardB"),
            new Card("CardC"),
            new Card("CardD"),
            new Card("CardE"),
            new Card("CardF"),
        });
 

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestEssential()
        {
            Assert.IsTrue(listA.IsEssential);
            Assert.IsFalse(listB.IsEssential);
        }
        /// <summary>
        /// Тест метода Get (получение карты из списка)
        /// </summary>
        [TestMethod]
        public void TestGet()
        {
            int findId = listA.All[0].Id;
            Assert.AreEqual("CardA", listA.Get(findId).Name);
        }
        /// <summary>
        /// Тест методов Search
        /// </summary>
        [TestMethod]
        public void TestSearch()
        {
            //Ищем карту А и сравниваем её имя с "CardA"
            Assert.AreEqual("CardA", listA.SearchByContent("A"));
            
        }
        /// <summary>
        /// Тест метода Sort
        /// </summary>
        [TestMethod]
        public void TestSort()
        {
            //Сортируем список A сравниваем порядок карт.
            //Как должно быть:B,F,D,E,A,C
            //(Не знаю как поведут себя карты с одинаковой важностью или выполнение)

        }
        /// <summary>
        /// Тест метода Set (сохранение карты в списке)
        /// </summary>
        [TestMethod]
        public void TestSet()
        {
            Card cCard = listA.All[0];
            //Меняем свойство карты A - важность или выполнение
            listA.Set(cCard);
        }
        /// <summary>
        /// Тест метода Contains (нахождение в списке)
        /// </summary>
        [TestMethod]
        public void TestContains()
        {
            Card cCard = listA.All[0];
            Assert.IsTrue(listA.Contains(cCard.Id));
        }
        /// <summary>
        /// Тест метода Remove (удаление из списка)
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            int ReId = listB.All[6].Id;
            listB.Remove(ReId);
            Assert.IsFalse(listB.Contains(ReId));
        }
    }
}