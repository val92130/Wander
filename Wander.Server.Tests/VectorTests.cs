using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.ClassLibrary.Model;

namespace Wander.Server.Tests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void CreateVectorNoParametersSetXAndYToZero()
        {
            Vector2 v = new Vector2();
            Assert.AreEqual(v.X, 0);
            Assert.AreEqual(v.Y, 0);
        }

        [TestMethod]
        public void CreateVectorParameters()
        {
            Vector2 v = new Vector2(10,15);
            Assert.AreEqual(v.X, 10);
            Assert.AreEqual(v.Y, 15);
        }

        [TestMethod]
        public void NormalizeVectorWorksCorrectly()
        {
            Vector2 v = new Vector2(10,15);
            v = Vector2.Normalize(v);

            Assert.AreEqual(v.Length(), 1);
        }

        [TestMethod]
        public void SumOfTwoVectorsWorksCorrectly()
        {
            Vector2 v = new Vector2(10,5);

            Vector2 n = v + new Vector2(20, 50);
            Assert.IsTrue((n.X == 30 && n.Y == 55));
        }
        [TestMethod]
        public void MultipliacationOfTwoVectorsWorksCorrectly()
        {
            Vector2 v = new Vector2(10, 5);

            Vector2 n = v * new Vector2(20, 50);
            Assert.IsTrue((n.X == 200 && n.Y == 250));
        }

        [TestMethod]
        public void TwoIdenticalVectorsAreEquals()
        {
            Vector2 v1 = new Vector2(50,80);
            Vector2 same = new Vector2(50,80);

            Assert.AreEqual(v1, same);
        }

        [TestMethod]
        public void TwoIdenticalVectorReturnsSameHashCode()
        {
            Vector2 v1 = new Vector2(50, 80);
            Vector2 same = new Vector2(50, 80);

            Assert.AreEqual(v1.GetHashCode(), same.GetHashCode());
        }
    }
}
