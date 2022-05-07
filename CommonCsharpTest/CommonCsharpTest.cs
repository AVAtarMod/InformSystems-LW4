using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SLE;
using System.Collections.Generic;
using System.Linq;

namespace VariousTests
{
    [TestClass]
    public class VariousTests
    {
        [TestMethod]
        public void ListResize()
        {
            List<int> source = new List<int>(), add = new List<int>();
            add.AddRange(new int[] { 1, 2, 3 });
            source.Add(3);

            source.AddRange(add);

            Assert.AreEqual(4, source.Count);
        }
        [TestMethod]
        public void ListRemoveMethod()
        {
            int[] src = new int[] { 1, 2, 3 };
            int[] res = new int[] { 1, 2 };
            List<int> list = new List<int>(src);

            // list.RemoveAt(2);
            list.RemoveRange(2, 1);
#if DEBUG
            Console.WriteLine();
            Console.Write("Src: ");
            src.ToList<int>().ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
            Console.Write("Exp res: ");
            res.ToList<int>().ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
            Console.Write("Res: ");
            list.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
#endif
            Assert.IsTrue(list.SequenceEqual(res));
        }
    }
}
