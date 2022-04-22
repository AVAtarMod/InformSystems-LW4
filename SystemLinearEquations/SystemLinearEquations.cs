using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLinearEquations
{
    [Serializable]
    public class UnsolvableSleException : Exception
    {
        public UnsolvableSleException() { }
        public UnsolvableSleException(string message) : base(message) { }
        public UnsolvableSleException(string message, Exception inner) : base(message, inner) { }
        protected UnsolvableSleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class SystemLinearEquations
    {
        private List<LinearEquation> data;
        private int length;
        public SystemLinearEquations(int variableAmount)
        {
            if (variableAmount > 0)
            {
                data = new List<LinearEquation>();
                length = variableAmount;
            }
            else throw new ArgumentException("Must be positive", "variableAmount");
        }
        public int Size => data.Count;
        public int Length => length;
    }
}
