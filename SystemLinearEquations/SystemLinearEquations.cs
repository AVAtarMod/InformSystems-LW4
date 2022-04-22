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
    }
}
