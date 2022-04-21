using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SystemLinearEquations;

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
        [TestMethod]
        public void ConstructFromString()
        {
            string Le = "1.5, 1.0, 4, 2";
            LinearEquation equation = new LinearEquation(Le);

            bool result = equation[0] == 1.5 && equation[1] = 1.0 && equation[2] == 4 && equation[3] = 2;

            Assert.IsTrue(result);
        }
        //TODO: Construct empty string
        [TestMethod]
        public void ConstructFromLE()
        {
            LinearEquation e1 = new LinearEquation(1.5, 1.0, 4, 2, 3);

            LinearEquation e2 = new LinearEquation(e1);

            Assert.AreEqual(e1, e2);
        }
        //TODO: Construct empty LE
        [TestMethod]
        public void ConstructFromArray()
        {
            double[] array = { 1.5, 1.0, 4, 2, 1, 0 };

            LinearEquation equation = new LinearEquation(array);

            Assert.AreEqual(array, equation.Data);
        }
        //TODO: Construct empty array
        [TestMethod]
        public void InitRandomNumbers()
        {
        }
        [TestMethod]
        public void InitSameNumbers()
        {
        }
        [TestMethod]
        public void OperatorSumLE()
        {
        }
        [TestMethod]
        public void OperatorSumLE_Zeros()
        {
        }
        [TestMethod]
        public void OperatorDiffLE()
        {
        }
        [TestMethod]
        public void OperatorDiffLE_Zeros()
        {
        }
        [TestMethod]
        public void OperatorMultToDouble_Left()
        {
        }
        [TestMethod]
        public void OperatorMultToDouble_Right()
        {
        }
        [TestMethod]
        public void OperatorMinus()
        {
        }
        [TestMethod]
        public void OperatorEqual_True()
        {
        }
        [TestMethod]
        public void OperatorEqual_False()
        {
        }
        [TestMethod]
        public void OperatorNotEqual_False()
        {
        }
        [TestMethod]
        public void OperatorFalse()
        {
        }
        [TestMethod]
        public void OperatorTrue()
        {
        }
        [TestMethod]
        public void ConversionToString()
        {
        }
        [TestMethod]
        public void ConversionToArray_Implicit()
        {
        }
    }
}
