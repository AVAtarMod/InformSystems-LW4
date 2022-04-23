using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SystemLinearEquations;
using System.Collections.Generic;

namespace SystemLinearEquationTest
{
    [TestClass]
    public class SystemLinearEquationTest
    {
        private bool isEchelon(List<List<double>> matrix)
        {
            int length = matrix[0].Count;
            int size = matrix.Count / length;

            for (int col = 0; col < size; col++)
                for (int row = col + 1; row < size; row++)
                    if (matrix[row][col] != 0) return false;

            return true;
        }
        [TestMethod]
        public void ConstructByVarNumbers()
        {
            int length = 5;

            SystemLinearEquations.SystemLinearEquations system = new SystemLinearEquations.SystemLinearEquations(length);

            Assert.AreEqual(5, system.Length);
        }
        [TestMethod]
        public void GetLeByIndex()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations.SystemLinearEquations system =
                new SystemLinearEquations.SystemLinearEquations(length);
            system.Add(Le1);
            system.Add(Le2);

            LinearEquation result = system[1];

            Assert.AreEqual(Le2, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowGetLeByIndex_Negative()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations.SystemLinearEquations system =
                new SystemLinearEquations.SystemLinearEquations(length);

            system.Add(Le1);
            system.Add(Le2);

            Assert.ThrowsException
                <ArgumentOutOfRangeException>(() => system[-1]);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowGetLeByIndex_GreaterThanMax()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations.SystemLinearEquations system =
                new SystemLinearEquations.SystemLinearEquations(length);

            system.Add(Le1);
            system.Add(Le2);

            Assert.ThrowsException
                <ArgumentOutOfRangeException>(() => system[system.Size + 1]);
        }
        [TestMethod]
        public void GetEchelonForm()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations.SystemLinearEquations system =
                new SystemLinearEquations.SystemLinearEquations(length);
            system.Add(Le1);
            system.Add(Le1);
            system.Add(Le2);
            system.Add(Le2);
            system.Add(Le2);

            system = system.ToEchelonForm();

            Assert.IsTrue(isEchelon(system));
        }
        [TestMethod]
        public void GetSolution()
        {
            double[] expectedResult = new double[]
                { 2 / 5, 1 / 4, -241 / 240, 19 / 12 };
            List<LinearEquation> equations = new List<LinearEquation>(new LinearEquation[] {
            new LinearEquation(new double[]{1.5, 5, 4, 2, 1 }),
            new LinearEquation(new double[]{4, 1, 4, 2, 1 }),
            new LinearEquation(new double[]{1.5, 2, 4, 5, 5 }),
            new LinearEquation(new double[]{1.5, 1, 4, 2, 0 }),
            new LinearEquation(new double[]{1.5, 1, 4, 2, 0 })
            });
            SystemLinearEquations.SystemLinearEquations system = new SystemLinearEquations.SystemLinearEquations(equations.Count - 1);
            foreach (LinearEquation Le in equations)
            {
                system.Add(Le);
            }

            double[] result = system.GetSolution();

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        [ExpectedException(typeof(UnsolvableSleException))]
        public void ThrowGetSolution()
        {
            double[] array = LinearEquationTest.zeroArray;
            LinearEquation Le = new LinearEquation(array);
            SystemLinearEquations.SystemLinearEquations system = new SystemLinearEquations.SystemLinearEquations(array.Length - 1);
            system.Add(Le);
            system.Add(Le);

            Assert.ThrowsException
                <UnsolvableSleException>(() => system.GetSolution());
        }
        [TestMethod]
        public void ConversionToString()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations.SystemLinearEquations system =
                new SystemLinearEquations.SystemLinearEquations(length);

            system.Add(Le1);
            system.Add(Le2);

            Assert.AreEqual(Le1.ToString() + '\n' + Le2.ToString(), system.ToString());
        }
    }
    [TestClass]
    public class LinearEquationTest
    {
        static internal double[] array6 = { 1.5, 1.0, 4, 2, 1, 0 };
        static internal double[] array4 = { 1.5, 1.0, 4, 2 };
        static internal double[] zeroArray = { 0, 0, 0, 0, 1 };
        static internal string array6Str = "1.5, 1.0, 4, 2, 1, 0";
        static internal string array4Str = "1.5, 1.0, 4, 2";

        private int GetCountDuplicates(LinearEquation equation)
        {
            List<ValueTuple<double, int>> duplicates = new List<ValueTuple<double, int>>();

            int length = equation.Length;
            for (int i = 0; i < length; i++)
            {
                double data = equation[i];
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
            LinearEquation equation = new LinearEquation(Le);

            bool result = equation[0] == 1.5 && equation[1] == 1.0 && equation[2] == 4 && equation[3] == 2;

            Assert.IsTrue(result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowConstructFromString_Empty()
        {
            string empty = "";

            LinearEquation equation = new LinearEquation(empty);

            Assert.ThrowsException
                <ArgumentOutOfRangeException>(() => equation[0]);
        }
        [TestMethod]
        public void ConstructFromLe()
        {
            LinearEquation e1 = new LinearEquation(array6);

            LinearEquation e2 = new LinearEquation(e1);

            Assert.AreEqual(e1, e2);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowConstructFromLe_Empty()
        {
            LinearEquation e1 = new LinearEquation();

            Assert.ThrowsException<ArgumentException>(() => new LinearEquation(e1));
        }
        [TestMethod]
        public void ConstructFromArray()
        {
            double[] array = array6;

            LinearEquation equation = new LinearEquation(array);

            Assert.AreEqual(array, equation.Data);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowConstructFromArray_Empty()
        {
            double[] array = new double[5];

            Assert.ThrowsException<ArgumentException>(() => new LinearEquation(array));
        }
        [TestMethod]
        public void InitRandomNumbers()
        {
            int length = 5;
            LinearEquation equation = new LinearEquation(length);

            equation.InitRandom(-10, 10);

            Assert.IsTrue(GetCountDuplicates(equation) <= 2);
        }
        [TestMethod]
        public void InitSameNumbers()
        {
            int length = 5, number = 1;
            LinearEquation equation = new LinearEquation(length);

            equation.Init(number);

            Assert.IsTrue(GetCountDuplicates(equation) == length);
        }
        [TestMethod]
        public void OperatorSumLe()
        {
            double[] arrayL = array6;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            LinearEquation result = l + r;

            Assert.IsTrue(result[0] == 3 && result[1] == 2 && result[2] == 8 && result[3] == 4 && result[4] == 1 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorSumLe_Zeros()
        {
            double[] arrayL = array6;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(5);

            LinearEquation result = l + r;

            Assert.AreEqual(l, result);
        }
        [TestMethod]
        public void OperatorDiffLe()
        {
            double[] arrayL = array6;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            LinearEquation result = l - r;

            Assert.IsTrue(result[0] == 0 && result[1] == 0 && result[2] == 0 && result[3] == 0 && result[4] == 1 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorDiffLe_Zeros()
        {
            double[] arrayL = array6;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(5);

            LinearEquation result = l - r;

            Assert.AreEqual(l, result);
        }
        [TestMethod]
        public void OperatorMultToDouble_Left()
        {
            double mult = 3;
            LinearEquation equation = new LinearEquation(array6);

            LinearEquation result = mult * equation;

            Assert.IsTrue(result[0] == 4.5 && result[1] == 3.3 && result[2] == 12 && result[3] == 6 && result[4] == 3 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorMultToDouble_Right()
        {
            double[] array = array6;
            double mult = 3;
            LinearEquation equation = new LinearEquation(array);

            LinearEquation result = equation * mult;

            Assert.IsTrue(result[0] == 4.5 && result[1] == 3.3 && result[2] == 12 && result[3] == 6 && result[4] == 3 && result[5] == 0);
        }
        [TestMethod]
        public void OperatorMinus()
        {
            double[] array = array6;
            LinearEquation equation = new LinearEquation(array);

            LinearEquation result = -equation;
            Assert.IsTrue(result[0] == -array[0] && result[1] == -array[1] && result[2] == --array[2] && result[3] == --array[3] && result[4] == --array[4] && result[5] == --array[5]);
        }
        [TestMethod]
        public void OperatorEqual_True()
        {
            double[] arrayL = array6;
            double[] arrayR = array6;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            bool result = l == r;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void OperatorEqual_False()
        {
            double[] arrayL = array6;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            bool result = l == r;

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void OperatorNotEqual_True()
        {
            double[] arrayL = array6;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            bool result = l != r;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void OperatorFalse()
        {
            double[] array = zeroArray;
            LinearEquation l = new LinearEquation(array);

            bool result = l ? true : false;

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void OperatorTrue()
        {
            double[] array = array6;
            LinearEquation l = new LinearEquation(array);

            bool result = l ? true : false; ;

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ConversionToString()
        {
            string Le = "1.5, 1, 4, 2";
            LinearEquation equation = new LinearEquation(array4);

            string result = equation.ToString();

            Assert.AreEqual(Le, result);
        }
        [TestMethod]
        public void ConversionToArray_Implicit()
        {
            LinearEquation equation = new LinearEquation(array4);

            List<double> result = (List<double>)equation;

            Assert.AreEqual(result.ToArray(), array4);
        }
    }
}
