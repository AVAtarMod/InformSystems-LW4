using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace SLE
{
    public class LinearEquationEnumerator : IEnumerator<double>
    {
        private double[] _data;
        private int _position = -1;
        private bool _isDisposed = false;

        public LinearEquationEnumerator(double[] linearEquationData)
        {
            _data = linearEquationData;
        }
        public double Current
        {
            get
            {
                _ = _data
                    ?? throw new InvalidOperationException();

                return _data[_position];
            }
        }
        object IEnumerator.Current
        {
            get { return this.Current; }
        }
        public bool MoveNext()
        {
            ++_position;
            return _position < _data.Length;
        }
        public void Reset() { _position = -1; }
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _data = null;
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
        ~LinearEquationEnumerator()
        {
            Dispose();
        }
    }
}
