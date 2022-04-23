using System;
using System.Collections.Generic;

namespace SLE
{
    public class LinearEquation
    {
        private List<double> data;
        private double b;
        private void ConstructFromArray(double[] array)
        {
            int length = array.Length;
            if (length > 1)
            {
                //! Make deep copy of the array
                data = new List<double>(array);
                data.RemoveAt(length - 1);
                b = array[length - 1];
            }
            else throw new ArgumentException("Length array must be >= 2", "array");
        }

        // Constructors
        public LinearEquation()
        {
            data = new List<double>();
            b = 0;
        }
        /// <summary>
        /// Allocate memory for equation of size variableAmount
        /// </summary>
        public LinearEquation(int variableAmount)
        {
            data = new List<double>(variableAmount - 1);
            b = 0;
        }
        public LinearEquation(string equation)
        {
            double[] array = Array.ConvertAll<string, double>
                (equation.Split(','),
                x => double.Parse(x));
            ConstructFromArray(array);
        }
        public LinearEquation(double[] array)
        {
            ConstructFromArray(array);
        }
        public LinearEquation(LinearEquation equation)
        {
            data = equation.data;
            b = equation.b;
        }

        // User methods
        public static class MergeActions
        {
            public static Func<double, double, double> Add = (x, y) => x + y;
            public static Func<double, double, double> Subtract = (x, y) => x + y;
            public static Func<double, double, double> Multiply = (x, y) => x * y;
        }
        public static LinearEquation Merge(LinearEquation first, LinearEquation second, Func<double, double, double> mergeAction)
        {
            if (first.Length > second.Length)
                second.Resize(first.Length);
            else
                first.Resize(second.Length);

            int length = first.Length;
            LinearEquation result = new LinearEquation(length);
            for (int i = 0; i < length; i++)
            {
                result[i] = mergeAction(first[i], second[i]);
            }
            return result;
        }
        static public class ForEachActions
        {
            public static Func<double, object, double> MultiplyDouble = (x, userData) => x * (double)userData;
        }
        public static LinearEquation ForEach(LinearEquation equation, Func<double, object, double> action, object userData)
        {
            int length = equation.Length;
            LinearEquation result = new LinearEquation(length);
            for (int i = 0; i < length; i++)
            {
                result[i] = action(equation[i], userData);
            }
            return result;
        }
        public class TrueForRangeActions
        {
            public static Func<double, object, bool> isEqual = (x, userData) => x == (double)userData;
            public static Func<double, object, bool> isNotEqual = (x, userData) => x == (double)userData;
        }
        public static TrueForRangeActions TrueForAllActions;
        public static bool TrueForRange(LinearEquation equation, int from, int to, Func<double, object, bool> action, object userData)
        {
            int length = equation.Length;
            if (from > 0 && to > 0 && from < to && to <= length)
            {
                for (int i = from; i < to; i++)
                {
                    if (!action(equation[i], userData))
                        return false;
                }
                return true;
            }
            else throw new ArgumentOutOfRangeException("from, to", "Invalid range");
        }
        public static bool TrueForAll(LinearEquation equation, Func<double, object, bool> action, object userData)
        {
            int length = equation.Length;
            for (int i = 0; i < length; i++)
            {
                if (!action(equation[i], userData))
                    return false;
            }
            return true;
        }
        public void Resize(int newSize)
        {
            int sizeDiff = newSize - Length;
            if (newSize < 0)
                throw new ArgumentException("Size Linear equation must be positive", "newSize");
            else if (newSize == 0)
                Clear();
            else if (sizeDiff > 0)
            {
                List<double> end = new List<double>(sizeDiff);
                data.InsertRange(Length, end);
            }
            else if (sizeDiff < 0)
            {
                data.RemoveRange(Length + sizeDiff, Math.Abs(sizeDiff));
            }
        }
        public void Clear()
        {
            data.Clear();
            b = 0;
        }
        public void InitRandom(double minValue, double maxValue)
        {
            Random PRNG = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < data.Count; i++)
            {
                double randomNumber = PRNG.NextDouble() * (maxValue - minValue) + minValue;
                data[i] = randomNumber;
            }
            b = PRNG.NextDouble() * (maxValue - minValue) + minValue;
        }
        /// <summary>
        /// Initialize equation by identical numbers
        /// </summary>
        /// <param name="value">Value for set</param>
        public void Init(double value)
        {
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = value;
            }
            b = value;
        }

        // Operators

        public double this[int index]
        {
            get
            {
                if (index < Length)
                    return data[index];
                else return b;
            }
            set
            {
                if (index < Length)
                    data[index] = value;
                else b = value;
            }
        }
        public static explicit operator List<double>(LinearEquation equation)
        {
            List<double> result = new List<double>(equation.data);
            result.Insert(equation.Length - 1, equation.b);
            return result;
        }
        public static LinearEquation operator +(LinearEquation left, LinearEquation right)
        {
            return Merge(left, right, MergeActions.Add);
        }
        public static LinearEquation operator -(LinearEquation left, LinearEquation right)
        {
            return Merge(left, right, MergeActions.Subtract);
        }
        public static LinearEquation operator *(LinearEquation left, double right)
        {
            return ForEach(left, ForEachActions.MultiplyDouble, right);
        }
        public static LinearEquation operator *(double left, LinearEquation right)
        {
            return ForEach(right, ForEachActions.MultiplyDouble, left);
        }
        public static LinearEquation operator -(LinearEquation right)
        {
            return ForEach(right, ForEachActions.MultiplyDouble, -1D);
        }
        public static bool operator true(LinearEquation equation)
        {
            return !(TrueForRange(equation, 0, equation.Length - 1, TrueForRangeActions.isEqual, 0) && equation.b != 0);
        }
        public static bool operator false(LinearEquation equation)
        {
            return TrueForRange(equation, 0, equation.Length - 1, TrueForRangeActions.isEqual, 0) && equation.b != 0;
        }
        // Accessors
        /// <summary>Return full length of LE (including constant term or 'b')</summary>
        public int Length => data.Count + 1;
        public double[] Data
        {
            get
            {
                List<double> result = data;
                data.Insert(Length - 1, b);
                return result.ToArray();
            }
        }
    }
}
