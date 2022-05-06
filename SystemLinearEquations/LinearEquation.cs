using System;
using System.Collections.Generic;
using System.Linq;

namespace SLE
{
    public class LinearEquation
    {
        private List<double> data;
        private double b;
        private bool empty = true;
        private void ConstructFromArray(double[] array)
        {
            int length = array.Length;
#if DEBUG
            Console.WriteLine("C1: " + (length > 1));
            Console.WriteLine("C2: " + (!array.All(x => x == 0)));
#endif

            if (length > 1 && !(array.All(x => x == 0)))
            {
                //! Make deep copy of the array
                data = new List<double>(array);
                data.RemoveAt(length - 1);
                b = array[length - 1];
                empty = false;
            }
            else
                throw new ArgumentException("Length array must be >= 2", "array");
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
            empty = false;
        }
        public LinearEquation(string equation)
        {
            double[] array;
            try
            {
                array = Array.ConvertAll<string, double>
                        (equation.Split(','),
                        x => double.Parse(x));
                ConstructFromArray(array);
            }
            catch (FormatException)
            {
                throw new ArgumentException();
            }
        }
        public LinearEquation(double[] array)
        {
            ConstructFromArray(array);
            empty = false;
        }
        public LinearEquation(LinearEquation equation)
        {
            data = equation.data;
            b = equation.b;
            empty = equation.empty;
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
            if (newSize < 0)
                throw new ArgumentException("Size Linear equation must be positive", "newSize");
            else if (newSize == 0)
                Clear();

            int sizeDiff = newSize - Length;

            if (sizeDiff > 0)
            {
                double[] end = new double[sizeDiff];
                data.AddRange(end);
                data[Length - 2] = b;
                b = 0;
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
                if (!empty)
                {
                    if (index < Length - 1)
                        return data[index];
                    else if (index == Length - 1)
                        return b;
                    else
                        throw new ArgumentOutOfRangeException();
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            set
            {
                if (!empty)
                {
                    if (index < Length - 1)
                        data[index] = value;
                    else if (index == Length - 1)
                        b = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
                else
                    throw new ArgumentOutOfRangeException();
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this == (LinearEquation)obj;
        }


        public static bool operator ==(LinearEquation left, LinearEquation right)
        {
            if (left.Length == right.Length)
            {
                for (int i = 0; i < left.Length; ++i)
                {
                    if (left[i] != right[i])
                        return false;
                }
                return true;
            }
            return false;
        }
        public static bool operator !=(LinearEquation left, LinearEquation right)
        {
            return !(left == right);
        }
        public static bool operator true(LinearEquation equation)
        {
            return !(TrueForRange(equation, 0, equation.Length - 1, TrueForRangeActions.isEqual, 0) && equation.b != 0);
        }
        public static bool operator false(LinearEquation equation)
        {
            return TrueForRange(equation, 0, equation.Length - 1, TrueForRangeActions.isEqual, 0) && equation.b != 0;
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Length; i++)
            {
                result += this[i].ToString() + ' ';
            }
            result.Remove(result.Length - 1);
            return result;
        }
        // Accessors
        /// <summary>Return full length of LE (including constant term i.e 'b')</summary>
        public int Length => data.Count + 1;
        public double[] Data
        {
            get
            {
                // double[] tmp = data.ToArray();
                List<double> result = new List<double>(data);
                result.Insert(Length - 1, b);
                return result.ToArray();
            }
        }
    }
}
