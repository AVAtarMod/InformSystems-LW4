using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4LW
{
    public class Indexer
    {
        public Indexer(double[] array, Index from, Index to)
        {
            if (from >= to)
                throw new ArgumentException("From must be lesser then to. From=" + from.ToString() + ", to=" + to.ToString());
            if (to > array.Length)
                throw new ArgumentException("End subarray is went out of array boundaries.");

            this.from = from;
            this.to = to;
            this.array = (double[])array.Clone();
            return;
        }
        public int Length => to - from + 1;
        public double this[Index i]
        {
            get
            {
                Index real = from + i;
                return array[real];
            }
            set
            {
                Index real = from + i;
                array[real] = value;
            }
        }

        private double[] array;
        private Index from, to;
    }
}
