using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4LW
{
    public class Indexer
    {
        public Indexer(double[] array, ArrayIndex from, ArrayIndex to)
        {
            if (from >= to)
                throw new ArgumentException("From must be lesser then to. From=" + from.ToString() + ", to=" + to.ToString());
            if (to > array.Length)
                throw new ArgumentException("End subarray is went out of array boundaries.");

            this.from = from;
            this.to = to;
            this.array = array;
            return;
        }
        public int Length => to - from + 1;
        public double this[ArrayIndex i]
        {
            get
            {
                ArrayIndex real = from + i;
                return (int)array.GetValue(real);
            }
            set
            {
                ArrayIndex real = from + i;
                array.SetValue(value, real);
            }
        }

        private Array array;
        private ArrayIndex from, to;
    }
}
