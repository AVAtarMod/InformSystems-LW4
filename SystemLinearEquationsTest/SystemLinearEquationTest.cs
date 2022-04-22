using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SystemLinearEquations;
using System.Collections.Generic;

namespace SystemLinearEquationTest
{
    [TestClass]
    public class SystemLinearEquationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
    [TestClass]
    public class LinearEquationTest
    {
        private double[] array5 = { 1.5, 1.0, 4, 2, 1, 0 };
        private double[] array4 = { 1.5, 1.0, 4, 2 };

        private int GetCountDuplicates(LinearEquation equation)
        {
            List<ValueTuple<double, int>> duplicates = new List<ValueTuple<double, int>>();

            int length = equation.Length;
            for (int i = 0; i < length; i++)
            {
                int data = equation[i];
                if (!duplicates.Exists(x => x.Item1 == data))
                {
                    duplicates.Add((data, 1));
                }
                else
                {
                    var tmp = duplicates.Find(x => x.Item1 == data);
                    ++tmp.Item2;
                }
            }

            int result = 0;
            foreach (var item in duplicates)
            {
                if (item.Item2 > 1) result += item.Item2;
            }

            return result;
        }
        [TestMethod]
        public void ConstructFromString()
        {
            string Le = "1.5, 1.0, 4, 2";
            LinearEquation equation = new LinearEquationе(Le);

            bool result = equation[0] == 1.5 && equation[1] = 1.0 && equation[2] == 4 && equation[3] = 2;

            Assert.IsTrue(result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowConstructFromString_Empty()
        {
            string Le = "";

            LinearEquation equation = new LinearEquationе(Le);

            Assert.ThrowsException
                <ArgumentOutOfRangeException>(equation[0]);
        }
        [TestMethod]
        public void ConstructFromLE()
        {
            LinearEquation e1 = new LinearEquation(1.5, 1.0, 4, 2, 3);

            LinearEquation e2 = new LinearEquation(e1);

            Assert.AreEqual(e1, e2);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowConstructFromLE_Empty()
        {
            LinearEquation e1 = new LinearEquation();

            Assert.ThrowsException<ArgumentException>(new LinearEquation(e1));
        }
        [TestMethod]
        public void ConstructFromArray()
        {
            double[] array = this.array5;

            LinearEquation equation = new LinearEquation(array);

            Assert.AreEqual(array, equation.Data);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowConstructFromArray_Empty()
        {
            double[] array = new double[5];

            Assert.ThrowsException<ArgumentException>(new LinearEquation(array));
        }
        [TestMethod]
        public void InitRandomNumbers()
        {
            int length = 5;
            LinearEquation equation = new LinearEquation(length);

            equation.Init(LinearEquation.Random);

            Assert.IsTrue(GetCountDuplicates(equation) <= 2);
        }
        [TestMethod]
        public void InitSameNumbers()
        {
            int length = 5;
            LinearEquation equation = new LinearEquation(length);

            equation.Init(LinearEquation.Same, 1);

            Assert.IsTrue(GetCountDuplicates(equation) == length);
        }
        [TestMethod]
        public void OperatorSumLE()
        {
            double[] arrayL = array5;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            LinearEquation result = l + r;

            Assert.IsTrue(result[0] == 3 && result[1] == 2 && result[2] == 8 && result[3] == 4 && result[4] == 1 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorSumLE_Zeros()
        {
            double[] arrayL = array5;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(5);

            LinearEquation result = l + r;

            Assert.AreEqual(l, result);
        }
        [TestMethod]
        public void OperatorDiffLE()
        {
            double[] arrayL = array5;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            LinearEquation result = l - r;

            Assert.IsTrue(result[0] == 0 && result[1] == 0 && result[2] == 0 && result[3] == 0 && result[4] == 1 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorDiffLE_Zeros()
        {
            double[] arrayL = array5;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(5);

            LinearEquation result = l - r;

            Assert.AreEqual(l, result);
        }
        [TestMethod]
        public void OperatorMultToDouble_Left()
        {
            double mult = 3;
            LinearEquation equation = new LinearEquation(array5);

            LinearEquation result = mult * equation;

            Assert.IsTrue(result[0] == 4.5 && result[1] == 3.3 && result[2] == 12 && result[3] == 6 && result[4] == 3 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorMultToDouble_Right()
        {
            double[] array = array5;
            double mult = 3;
            LinearEquation equation = new LinearEquation(array);

            LinearEquation result = equation * mult;

            Assert.IsTrue(result[0] == 4.5 && result[1] == 3.3 && result[2] == 12 && result[3] == 6 && result[4] == 3 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorMinus()
        {
            double[] array = array5;
            LinearEquation equation = new LinearEquation(array);

            LinearEquation result = -equation;
            Assert.IsTrue(result[0] == -array[0] && result[1] == -array[1] && result[2] == --array[2] && result[3] == --array[3] && result[4] == --array[4] && result[5] == --array[5]);
        }
        [TestMethod]
        public void OperatorEqual_True()
        {
            double[] arrayL = array5;
            double[] arrayR = array5;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            bool result = l == r;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void OperatorEqual_False()
        {
            double[] arrayL = array5;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            bool result = l == r;

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void OperatorNotEqual_True()
        {
            double[] arrayL = array5;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            bool result = l != r;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void OperatorFalse()
        {
            double[] array = { 0, 0, 0, 0, 1 };
            LinearEquation l = new LinearEquation(array);

            bool result = array;

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void OperatorTrue()
        {
            double[] array = array5;
            LinearEquation l = new LinearEquation(array);

            bool result = array;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ConversionToString()
        {
            string Le = "1.5, 1, 4, 2";
            LinearEquation equation = new LinearEquationе(array4);

            string result = equation.ToString();

            Assert.AreEqual(Le, result);
        }
        [TestMethod]
        public void ConversionToArray_Implicit()
        {
            LinearEquation equation = new LinearEquationе(array4);

            double[] result = equation;

            Assert.AreEqual(result, array4);
        }
    }
}
