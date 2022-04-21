using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4LW
{
    public class ArrayIndex
    {
        private int index;
        public ArrayIndex(int index)
        {
            if (index >= 0)
                this.index = index;
            else throw new IndexOutOfRangeException("Index must be positive. We get " + index.ToString() + ". ");
        }
        public static implicit operator int(ArrayIndex i) => i.index;
        public static implicit operator ArrayIndex(int i) => new ArrayIndex(i);
    }
}
