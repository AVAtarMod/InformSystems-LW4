using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using _4LW;

namespace IndexerTest
{
    [TestClass]
    public class IndexerTest
    {
        double[] array = new double[] { 1, 2, 3, 4 };
        [TestMethod]
        public void HaveCorrectLength()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.AreEqual(2, indexer.Length);
        }
        [TestMethod]
        public void GetCorrectly()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.AreEqual(2, indexer[0]);
            Assert.AreEqual(3, indexer[1]);
        }
        [TestMethod]
        public void SetCorrectly()
        {
            var indexer = new Indexer(array, 1, 2);
            indexer[0] = 10;
            Assert.AreEqual(10, array[1]);
        }
        [TestMethod]
        public void IndexerDoesNotCopyArray()
        {
            var indexer1 = new Indexer(array, 1, 2);
            var indexer2 = new Indexer(array, 0, 2);
            indexer1[0] = 100500;
            Assert.AreEqual(100500, indexer2[1]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FailWithWrongArguments1()
        {
        Assert.Equals(typeof(ArgumentException), new Indexer(array, -1, 3));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FailWithWrongArguments2()
        {
            Assert.Equals(typeof(ArgumentException), new Indexer(array, 1, -1));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FailWithWrongArguments3()
        {
            Assert.Equals(typeof(ArgumentException), new Indexer(array, 1, 10));
        }
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FailWithWrongIndexing1()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.Equals(typeof(IndexOutOfRangeException), indexer[-1]);
        }
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FailWithWrongIndexing2()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.AreNotEqual(typeof(IndexOutOfRangeException), indexer[10]);
        }
    }
}
