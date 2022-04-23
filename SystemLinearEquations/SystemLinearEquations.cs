using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLinearEquations
{
    public class SystemLinearEquations
    {
        private List<LinearEquation> data;
        private int length;
        private void ConvertToEchelonForm(int currentCollumn, List<int> availableRows)
        {
            //TODO: mult Le to const, Le to Le actions
            //! Recursive algorhitm
        }
        public SystemLinearEquations(int variableAmount)
        {
            if (variableAmount > 0)
            {
                data = new List<LinearEquation>();
                length = variableAmount;
            }
            else throw new ArgumentException("Must be positive", "variableAmount");
        }
        public void Add(LinearEquation equation)
        {
            if (equation.Length == length)
                data.Add(equation);
            else
                throw new ArgumentException("Equation must be same size as system's Length", "equation");
        }
        public SystemLinearEquations ToEchelonForm()
        {
            SystemLinearEquations system = this;
            List<int> rows = new List<int>(Size);

            int length = Size;
            for (int i = 0; i < length; i++)
            {
                rows[i] = i;
            }

            system.ConvertToEchelonForm(0, rows);
            return system;
        }
        public double[] GetSolution()
        {
            return new double[] { 1, 2, 3 };
        }
        public LinearEquation this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
        public static implicit operator List<List<double>>(SystemLinearEquations system)
        {
            List<List<double>> result = new List<List<double>>();
            foreach (List<double> item in system.data)
            {
                result.Add(item);
            }
            return result;
        }
        public int Size => data.Count;
        public int Length => length;
    }
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
}
