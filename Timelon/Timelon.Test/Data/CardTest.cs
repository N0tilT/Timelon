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
            if (cardB.IsImportant == true) cardB.IsImportant = false;
            else cardB.IsCompleted = true;
            Assert.AreEqual(false, cardB.IsImportant);

            Assert.AreEqual(true, cardA.IsImportant);
            if (cardA.IsImportant == true) cardA.IsImportant = false;
            else cardA.IsCompleted = true;
            Assert.AreEqual(false, cardA.IsImportant);
        }
        /// <summary>
        /// Тест доступа к статусу выполнения
        /// </summary>
        [TestMethod]
        public void TestCompleted()
        {
            Assert.AreEqual(false, cardB.IsCompleted);
            if (cardB.IsCompleted == true) cardB.IsCompleted = false;
            else cardB.IsCompleted = true;
            Assert.AreEqual(true, cardB.IsCompleted);

            Assert.AreEqual(true, cardA.IsCompleted);
            if (cardA.IsCompleted == true) cardA.IsCompleted = false;
            else cardA.IsCompleted = true;
            Assert.AreEqual(false, cardA.IsCompleted);
        }
    }
}