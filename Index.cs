using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4LW
{
    public class Index
    {
        private int index;
        public Index(int index)
        {
            if (index >= 0)
                this.index = index;
            else throw new ArgumentException("Index must be positive. We get " + index.ToString() + ". ");
        }
        public static implicit operator int(Index i) => i.index;
        public static implicit operator Index(int i) => new Index(i);
    }
}
