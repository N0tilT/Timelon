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

            /// <summary>
             /// Строчное представление имен карт, хранящихся в списке
             /// </summary>
             /// <param name="list">Список карт</param>
             /// <returns>список имен карт</returns>
            public string CardsToStringGeneric(List<Card> list)
            {
                string s = "";

                foreach (Card item in list)
                    s += item.Name + " ";

                return s;
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
        private TestCardList listQ = new TestCardList(0, "EssentialTestListA", true, new List<Card>
        {
            new Card(6,"CardA",DateTimeContainer.Now,"This is CardA", true,true),
            new Card(7,"CardB",DateTimeContainer.Now,"This is CardB", true,false),
            new Card(8,"CardC",DateTimeContainer.Now,"This is CardC", false,true),
            new Card(9,"CardD",DateTimeContainer.Now,"This is CardD", false,false),
            new Card(10,"CardE",DateTimeContainer.Now,"This is CardE", false,false),
            new Card(11,"CardF",DateTimeContainer.Now,"This is CardF", true,false),
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

        /// <summary>
        /// Тест свойства "по умолчанию" у карт
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

            Assert.AreEqual("CardA", listQ.Get(6).Name);
        }
        
        /// <summary>
        /// Тест методов Search
        /// </summary>
        [TestMethod]
        public void TestSearch()
        {
            Assert.AreEqual("CardA ", listA.CardsToStringGeneric(listA.SearchByContent("CardA")));
            Assert.AreEqual("CardB ", listA.CardsToStringGeneric(listA.SearchByContent("B")));
            Assert.AreEqual("CardA CardB CardC CardD CardE CardF ", listA.CardsToStringGeneric(listA.SearchByContent("A")));
            Assert.AreEqual("CardA CardB CardC CardD CardE CardF ", listA.CardsToStringGeneric(listA.SearchByContent("a")));
            Assert.AreEqual("", listA.CardsToStringGeneric(listA.SearchByContent("137")));
            
        }

        /// <summary>
        /// Тест методов сортировки карт
        /// </summary>
        [TestMethod]
        public void TestSort()
        {
            Assert.AreEqual("CardB CardF ", listA.CardsToStringGeneric(listA.GetListImportant()));
            Assert.AreEqual("CardD CardE ", listA.CardsToStringGeneric(listA.GetListDefault()));
            Assert.AreEqual("CardA CardC ", listA.CardsToStringGeneric(listA.GetListCompleted()));
            Assert.AreEqual("CardB CardF CardD CardE CardA CardC ", 
                listA.CardsToStringGeneric(listA.GetListImportant()) + 
                listA.CardsToStringGeneric(listA.GetListDefault()) + 
                listA.CardsToStringGeneric(listA.GetListCompleted()));
            //CardB CardF CardD CardE CardC CardA 
        }
        /// <summary>
        /// Тест метода Set (сохранение карты в списке)
        /// </summary>
        [TestMethod]
        public void TestSet()
        {
            Card cCard = listQ.All[6];
            cCard.IsImportant = false;
            cCard.IsCompleted = false;
            listQ.Set(cCard);
            Assert.AreEqual(false, cCard.IsImportant);
            Assert.AreEqual(false, cCard.IsCompleted);
        }
        /// <summary>
        /// Тест метода Contains (нахождение в списке)
        /// </summary>
        [TestMethod]
        public void TestContains()
        {
            Card cCard = listQ.All[6];
            Assert.IsTrue(listQ.Contains(cCard.Id));
            Assert.IsFalse(listQ.Contains(0));

            Card bCard = listA.All[5];
            Assert.IsTrue(listA.Contains(bCard.Id));
            Assert.IsTrue(listA.Contains(1));
        }
        /// <summary>
        /// Тест метода Remove (удаление из списка)
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            int ReId = listQ.All[11].Id;
            listQ.Remove(ReId);
            Assert.IsFalse(listQ.Contains(ReId));

            int Re1Id = listA.All[0].Id;
            listA.Remove(Re1Id);
            Assert.IsFalse(listA.Contains(Re1Id));
        }
    }
}