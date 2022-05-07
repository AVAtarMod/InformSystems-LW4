using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Extensions;

namespace SLE
{
    public class LinearEquation : IEnumerable<double>
    {
        // Private data and behaviour
        private List<double> _data;
        private double _b;
        private bool _empty = true;
        private void ConstructFromArray(double[] array)
        {
            int length = array.Length;
#if DEBUG
            Console.WriteLine($"Ctr(double[]) conditions:  length > 1 = {length > 1}, !AllZero = {!array.All(x => x == 0)}");
#endif

            if (length > 1 && !(array.All(x => x == 0)))
            {
                //! Make deep copy of the array
                _data = new List<double>(array);
                _b = _data[length - 1];
                _data.RemoveAt(length - 1);
                _empty = false;
            }
            else
                throw new ArgumentException("Length array must be >= 2", "array");
        }
        private IEnumerator<double> GetEnumeratorPrivate()
        {
            return new LinearEquationEnumerator(Data);
        }

        // Constructors
        public LinearEquation()
        {
            _data = new List<double>();
            _b = 0;
        }
        /// <summary>
        /// Allocate memory for equation of size variableAmount
        /// </summary>
        public LinearEquation(int variableAmount)
        {
#if DEBUG
            Console.WriteLine("Using ctr(int)");
#endif
            _data = new List<double>(new double[variableAmount - 1]);
            _b = 0;
            _empty = false;
        }
        public LinearEquation(string equation)
        {
            double[] array;
            try
            {
                array = Array.ConvertAll<string, double>
                        (equation.Split(','),
                        x => double.Parse(x));
            }
            catch (FormatException)
            {
                throw new ArgumentException();
            }
            ConstructFromArray(array);
        }
        public LinearEquation(double[] array)
        {
            ConstructFromArray(array);
            _empty = false;
        }
        public LinearEquation(LinearEquation equation)
        {
            _data = equation._data;
            _b = equation._b;
            _empty = equation._empty;
        }

        // User methods
        public static class MergeActions
        {
            public static Func<double, double, double> Add = (x, y) => x + y;
            public static Func<double, double, double> Subtract = (x, y) => x - y;
            public static Func<double, double, double> Multiply = (x, y) => x * y;
        }
        public static LinearEquation Merge(LinearEquation first, LinearEquation second, Func<double, double, double> mergeAction)
        {
            if (first.Length > second.Length)
                second.Resize(first.Length);
            else
                first.Resize(second.Length);
#if DEBUG
            Console.WriteLine("first: " + first);
            Console.WriteLine("second: " + second);
#endif
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
#if DEBUG
            Console.WriteLine($"TrueForRange conditions: from > 0 = {from >= 0}, to > 0 = {to > 0}, from < to = {from < to}, to <= length = {to <= length}");
#endif
            if (from >= 0 && to > 0 && from < to && to <= length)
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
                int listEndIndex = Length - 1;
                double[] end = new double[sizeDiff];
                _data.AddRange(end);
                _data[listEndIndex] = _b;
                _b = 0;
            }

            else if (sizeDiff < 0)
            {
                _data.RemoveRange(Length + sizeDiff, Math.Abs(sizeDiff));
            }
        }
        public void Clear()
        {
            _data.Clear();
            _b = 0;
        }
        public void InitRandom(double minValue, double maxValue)
        {
            Random PRNG = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < _data.Count; i++)
            {
                double randomNumber = PRNG.NextDouble() * (maxValue - minValue) + minValue;
                _data[i] = randomNumber;
            }
            _b = PRNG.NextDouble() * (maxValue - minValue) + minValue;
        }
        /// <summary>
        /// Initialize equation by identical numbers
        /// </summary>
        /// <param name="value">Value for set</param>
        public void Init(double value)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                _data[i] = value;
            }
            _b = value;
        }
        public static bool IsSolvable(LinearEquation equation)
        {
            bool allZero = TrueForRange(equation, 0, equation.Length - 1, TrueForRangeActions.isEqual, (double)0);
#if DEBUG
            Console.WriteLine($"IsSolvable conditions: allZero = {allZero}, b!=0 = {equation._b != 0}");
#endif
            return !(allZero && equation._b != 0);
        }

        // Operators
        public IEnumerator<double> GetEnumerator()
        {
            return GetEnumeratorPrivate();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumeratorPrivate();
        }
        public double this[int index]
        {
            get
            {
                if (!_empty)
                {
                    if (index < Length - 1)
                        return _data[index];
                    else if (index == Length - 1)
                        return _b;
                    else
                        throw new ArgumentOutOfRangeException();
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            set
            {
                if (!_empty)
                {
                    if (index < Length - 1)
                        _data[index] = value;
                    else if (index == Length - 1)
                        _b = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static explicit operator EList<double>(LinearEquation equation)
        {
            EList<double> result = new EList<double>(equation._data);
            result.Insert(equation.Length - 1, equation._b);
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
            return IsSolvable(equation);
        }
        public static bool operator false(LinearEquation equation)
        {
            return !IsSolvable(equation);
        }
        public override string ToString()
        {
            string result = "";
            const string separator = ", ";
            for (int i = 0; i < Length; i++)
            {
                result += this[i].ToString() + separator;
            }
            return result.Remove(result.Length - separator.Length);
        }
        // Accessors
        /// <summary>Return full length of LE (including constant term i.e 'b')</summary>
        public int Length => _data.Count + 1;
        public double[] Data
        {
            get
            {
                List<double> result = new List<double>(_data);
                result.Add(_b);
                return result.ToArray();
            }
        }
    }
}
