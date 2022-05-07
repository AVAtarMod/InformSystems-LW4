using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SLE;
using System.Collections.Generic;
using System.Linq;
using Extensions;

namespace SystemLinearEquationTest
{
    [TestClass]
    public class SystemLinearEquationTest
    {
        private bool isEchelon(EList<EList<double>> matrix)
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

            SystemLinearEquations system = new SystemLinearEquations(length);

            Assert.AreEqual(5, system.Length);
        }
        [TestMethod]
        public void GetLeByIndex()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations system =
                new SystemLinearEquations(length);
            Console.WriteLine("LE length = " + Le2.Length + ", SLE length = " + length);
            system.Add(Le1);
            system.Add(Le2);

            LinearEquation result = system[1];

            Assert.AreEqual(Le2, result);
        }
        [TestMethod]
        public void ThrowGetLeByIndex_Negative()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations system =
                new SystemLinearEquations(length);

            system.Add(Le1);
            system.Add(Le2);

            Assert.ThrowsException
                <ArgumentOutOfRangeException>(() => system[-1]);
        }
        [TestMethod]
        public void ThrowGetLeByIndex_GreaterThanMax()
        {
            int length = 5;
            LinearEquation Le1 = new LinearEquation(LinearEquationTest.array6);
            LinearEquation Le2 = new LinearEquation(LinearEquationTest.array4);
            Le2.Resize(length);
            SystemLinearEquations system =
                new SystemLinearEquations(length);

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
            SystemLinearEquations system =
                new SystemLinearEquations(length);
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
            EList<LinearEquation> equations = new EList<LinearEquation>(new LinearEquation[] {
            new LinearEquation(new double[]{1.5, 5, 4, 2, 1 }),
            new LinearEquation(new double[]{4, 1, 4, 2, 1 }),
            new LinearEquation(new double[]{1.5, 2, 4, 5, 5 }),
            new LinearEquation(new double[]{1.5, 1, 4, 2, 0 }),
            new LinearEquation(new double[]{1.5, 1, 4, 2, 0 })
            });
            SystemLinearEquations system = new SystemLinearEquations(equations.Count - 1);
            foreach (LinearEquation Le in equations)
            {
                system.Add(Le);
            }

            double[] result = system.GetSolution();

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void ThrowGetSolution()
        {
            double[] array = LinearEquationTest.falseEquationArray;
            LinearEquation Le = new LinearEquation(array);
            SystemLinearEquations system = new SystemLinearEquations(array.Length - 1);
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
            SystemLinearEquations system =
                new SystemLinearEquations(length);

            system.Add(Le1);
            system.Add(Le2);

            Assert.AreEqual(Le1.ToString() + '\n' + Le2.ToString(), system.ToString());
        }
    }
    [TestClass]
    public class LinearEquationTest
    {
        static internal readonly double[] array6 = { 1.5, 1.0, 4, 2, 1, 0 };
        static internal readonly double[] array4 = { 1.5, 1.0, 4, 2 };
        static internal readonly double[] falseEquationArray = { 0, 0, 0, 0, 1 };
        static internal readonly string array6Str = "1.5, 1.0, 4, 2, 1, 0";
        static internal readonly string array4Str = "1.5, 1.0, 4, 2";
        private static EList<double> ListMult(EList<double> source, double mult)
        {
            EList<double> expected = new EList<double>(array6);
            for (int i = 0; i < expected.Count; ++i)
            {
                expected[i] *= mult;
            }

            return expected;
        }
        private int GetCountDuplicates(LinearEquation equation)
        {
            EList<(double, int)> duplicates = new EList<(double, int)>();

            int length = equation.Length;
            for (int i = 0; i < length; i++)
            {
                double data = equation[i];
#if DEBUG
                Console.Write("data: " + data + ", ");
#endif
                if (!duplicates.Exists(x => x.Item1 == data))
                {
#if DEBUG
                    Console.Write("add " + data + ", ");
#endif
                    duplicates.Add((data, 1));
                }
                else
                {
                    int index = duplicates.FindIndex(x => x.Item1 == data);
                    (double, int) tmp = duplicates[index];
                    ++tmp.Item2;
                    duplicates[index] = tmp;
                }
            }

            int result = 0;
            foreach (var item in duplicates)
            {
#if DEBUG
                Console.WriteLine();
                Console.Write("(" + item.Item1 + "; " + item.Item2 + "), ");
#endif
                if (item.Item2 > 1) result += item.Item2;
            }

            return result;
        }
        [TestMethod]
        public void ConstructFromString()
        {
            string LEstring = array4Str;

            LinearEquation result = new LinearEquation(LEstring);
#if DEBUG
            Console.WriteLine("Result: " + result);
            Console.WriteLine("Expected: " + array4.ToEList());
#endif
            Assert.IsTrue(array4.SequenceEqual(result));
        }
        [TestMethod]
        public void ThrowConstructFromString_Empty()
        {
            string empty = "";

            Assert.ThrowsException
                <ArgumentException>(() => new LinearEquation(empty));
        }
        [TestMethod]
        public void ConstructFromLe()
        {
            LinearEquation e1 = new LinearEquation(array6);

            LinearEquation e2 = new LinearEquation(e1);

            Assert.AreEqual(e1, e2);
        }
        [TestMethod]
        public void ConstructFromArray()
        {
            double[] array = array6;

            LinearEquation equation = new LinearEquation(array);
            Assert.IsTrue(array.SequenceEqual(equation.Data));
        }
        [TestMethod]
        public void ThrowConstructFromArray_Empty()
        {
            double[] array = new double[5];

#if DEBUG
            foreach (var item in array)
            {
                Console.Write(item.ToString() + ' ');
            }
            Console.WriteLine();
#endif

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
#if DEBUG
            Console.WriteLine("EMPTY: " + equation.ToString());
#endif
            equation.Init(number);
#if DEBUG
            Console.WriteLine("INIT: " + equation.ToString());
#endif
            Assert.IsTrue(GetCountDuplicates(equation) == length);
        }
        [TestMethod]
        public void ResizeLE()
        {
            int oldLength = array4.Length, newLength = 6;
            LinearEquation linear = new LinearEquation(array4);
            LinearEquation expected = new LinearEquation(new double[] { 1.5, 1.0, 4, 2, 0, 0 });
#if DEBUG
            Console.WriteLine("INIT: " + linear);
#endif
            linear.Resize(newLength);
#if DEBUG
            Console.WriteLine("RESIZED: " + linear);
#endif
            Assert.IsTrue(expected == linear);
        }
        [TestMethod]
        public void OperatorSumLe()
        {
            double[] arrayL = array6;
            double[] arrayR = array4;
            LinearEquation l = new LinearEquation(arrayL);
            LinearEquation r = new LinearEquation(arrayR);

            LinearEquation result = l + r;
#if DEBUG
            Console.WriteLine("l: " + l);
            Console.WriteLine("r: " + r);
            Console.WriteLine("l + r: " + (l + r));
#endif
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

#if DEBUG
            Console.WriteLine("l: " + l);
            Console.WriteLine("r: " + r);
            Console.WriteLine("l - r: " + (l - r));
#endif
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
            EList<double> expected = ListMult(new EList<double>(array6), mult);

            LinearEquation result = mult * equation;

#if DEBUG
            Console.WriteLine("l: " + mult);
            Console.WriteLine("r: " + equation);
            Console.WriteLine("l * r: " + (mult * equation));
            Console.Write("expected: " + expected);
#endif
            Assert.IsTrue(expected.SequenceEqual(result));
        }
        [TestMethod]
        public void OperatorMultToDouble_Right()
        {
            double[] array = array6;
            double mult = 3;
            LinearEquation equation = new LinearEquation(array);
            EList<double> expected = ListMult(array.ToEList(), mult);

            LinearEquation result = equation * mult;

            Assert.IsTrue(expected.SequenceEqual(result));
        }
        [TestMethod]
        public void OperatorMinus()
        {
            double[] array = array6;
            LinearEquation equation = new LinearEquation(array);
            EList<double> expected = ListMult(array.ToEList(), -1);

            LinearEquation result = -equation;

            Assert.IsTrue(result.SequenceEqual(result));
        }
        [TestMethod]
        public void OperatorEqual_True()
        {
            LinearEquation l = new LinearEquation(array6);
            LinearEquation r = new LinearEquation(array6);

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
            double[] array = falseEquationArray;
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

#if DEBUG
            Console.WriteLine($"Result: |{result}|");
            Console.WriteLine($"Expected: |{Le}|");
#endif
            Assert.AreEqual(Le, result);
        }
        [TestMethod]
        public void ConversionToArray_Implicit()
        {
            LinearEquation equation = new LinearEquation(array4);

            EList<double> result = (EList<double>)equation;

            Assert.IsTrue(array4.SequenceEqual(equation));
        }
        [TestMethod]
        public void LEgetType()
        {
            LinearEquation equation = new LinearEquation(new double[] { 1, 3, 4 });
            Assert.AreEqual(typeof(LinearEquation), equation.GetType());
        }
    }
}
