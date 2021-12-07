using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuthTestingTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string resultado = AuthTesting.Program.Something();

            Assert.AreEqual("Algo", resultado);
        }

        [TestMethod]
        public void TestLoginTrue()
        {
            bool resultado = AuthTesting.Program.Login("miguel","12345");

            Assert.AreEqual(true, resultado);
        }

        [TestMethod]
        public void TestLoginFalse()
        {
            bool resultado = AuthTesting.Program.Login("miguel", "12367");

            Assert.AreEqual(false, resultado);
        }
    }
}
