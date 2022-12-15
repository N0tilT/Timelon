using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timelon.Data;

namespace Timelon.Test.Data
{
    /// <summary>
    /// Модульное тестирование класса Card
    /// </summary>
    [TestClass]
    public class CardTest
    {
        public class TestCard : Card
        {
            public TestCard(int cardId, string cardName, DateTimeContainer date, string description, bool isImportant, bool isCompleted) : base(cardId, cardName, date, description, isImportant, isCompleted)
            {
            }
        }

        private TestCard cardA = new TestCard(0,"TestCardA",new DateTimeContainer(new System.DateTime(2022, 8, 10)),"This is the test card A",true,true);

        private TestCard cardB = new TestCard(1, "TestCardB", new DateTimeContainer(new System.DateTime(2022, 12, 23)), "This is the test card B", true, false);

        private TestCard cardC = new TestCard(2, "TestCardC", new DateTimeContainer(new System.DateTime(2022, 9, 4)), "This is the test card C", false, true);

        private TestCard cardD = new TestCard(3, "TestCardD", new DateTimeContainer(new System.DateTime(2022, 4, 9)), "This is the test card D", false, false);

        /// <summary>
        /// Тест доступа к уникальному индификатору карты
        /// </summary>
        [TestMethod]
        public void TestId()
        {
            Assert.AreEqual(0, cardA.Id);
            Assert.AreEqual(1, cardB.Id);
            Assert.AreEqual(2, cardC.Id);
            Assert.AreEqual(3, cardD.Id);
        }
        /// <summary>
        /// Тест доступа к имени карты
        /// </summary>
        [TestMethod]
        public void TestName()
        {
            Assert.AreEqual("TestCardA", cardA.Name);
            cardA.Name = "TestedCardA";
            Assert.AreEqual("TestedCardA", cardA.Name);
        }
        /// <summary>
        /// Тест доступа к дате карты
        /// </summary>
        [TestMethod]
        public void TestDate()
        {
            Assert.AreEqual(new System.DateTime(2022, 8, 10), cardA.Date.Created);
            Assert.AreEqual(new System.DateTime(2022, 12, 23), cardB.Date.Created);
        }
        /// <summary>
        /// Тест доступа к описанию карты
        /// </summary>
        [TestMethod]
        public void TestDescription()
        {
            Assert.AreEqual("This is the test card A", cardA.Description);
            cardA.Description = "This is tested card A";
            Assert.AreEqual("This is tested card A", cardA.Description);
        }
        /// <summary>
        /// Тест доступа к важности карты
        /// </summary>
        [TestMethod]
        public void TestImportance()
        {
            Assert.AreEqual(true, cardB.IsImportant);
            cardB.IsImportant = cardB.IsImportant != true;
            Assert.AreEqual(false, cardB.IsImportant);

            Assert.AreEqual(true, cardA.IsImportant);
            cardA.IsImportant = cardA.IsImportant != true;
            Assert.AreEqual(false, cardA.IsImportant);
        }
        /// <summary>
        /// Тест доступа к статусу выполнения
        /// </summary>
        [TestMethod]
        public void TestCompleted()
        {
            Assert.AreEqual(false, cardB.IsCompleted);
            cardB.IsCompleted = cardB.IsCompleted != true;
            Assert.AreEqual(true, cardB.IsCompleted);

            Assert.AreEqual(true, cardA.IsCompleted);
            cardA.IsCompleted = cardA.IsCompleted != true;
            Assert.AreEqual(false, cardA.IsCompleted);
        }
    }
}