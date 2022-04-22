using System;
using System.Collections.Generic;

namespace SystemLinearEquations
{
    public class LinearEquation
    {
        private List<double> data;
        private double b;
        public LinearEquation(double[] array)
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
        public LinearEquation(LinearEquation equation)
        {
            data = equation.data;
            b = equation.b;
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
            }
        }
        public void Clear()
        {
            data.Clear();
            b = 0;
        }
        /// <summary>Return full length of LE (including constant term or 'b')</summary>
        public int Length => data.Count + 1;
    }
}
