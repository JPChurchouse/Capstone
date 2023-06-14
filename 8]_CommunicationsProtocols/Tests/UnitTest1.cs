using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using XKarts.Identifier;
namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void testCreateKart()
        {
            XKarts.Identifier.Kart kart = new XKarts.Identifier.Kart(1, XKarts.Identifier.Colour.Red);
            Console.Write(kart.ToString());

            Assert.IsTrue((ulong)kart.Colour == 0xFF000);
            Assert.IsTrue(kart.ID == 1);
        }
    }
}