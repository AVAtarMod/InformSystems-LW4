using System;

namespace _4LW
{
    public class Indexer
    {
        public Indexer(double[] array, int from, int to)
        {
            ValidateRange<ArgumentException>(from, to, array.Length);

            this.from = from;
            this.to = to;
            this.array = array;

            return;
        }
        public int Length => to - from + 1;
        public double this[int i]
        {
            get
            {
                ValidateIndex<IndexOutOfRangeException>(i);
                int real = from + i;
                return (double)array.GetValue(real);
            }
            set
            {
                ValidateIndex<IndexOutOfRangeException>(i);
                int real = from + i;
                array.SetValue(value, real);
            }
        }

        private Array array;
        private int from = 0, to = 0;

        /// <summary>
        /// Throw Exception of type T if range is invalid.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <exception cref="Exception">default</exception>
        /// <exception cref="T">user-defined</exception>
        private void ValidateRange<T>(int from, int to, int length) where T : Exception, new()
        {
            ValidateIndexSign<T>(from);
            ValidateIndexSign<T>(to);
            if (from > to)
                throw new T();
            if (to > length)
                throw new T();

            return;
        }
        /// <summary>
        /// Throw Exception of type T if index is invalid.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <exception cref="Exception">default</exception>
        /// <exception cref="T">user-defined</exception>
        private void ValidateIndex<T>(int index) where T : Exception, new()
        {
            ValidateIndexSign<T>(index);
            if (index > Length)
                throw new T();
        }
        /// <summary>
        /// Throw Exception of type T if index negative
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <exception cref="Exception">default</exception>
        /// <exception cref="T">user-defined</exception>
        private void ValidateIndexSign<T>(int index) where T : Exception, new()
        {
            if (index < 0)
                throw new T();
        }
    }
}
