using NUnit.Framework;

namespace NUnitTestHexagono
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            TestHexagono.test_all();
            Assert.Pass();
        }
    }
}