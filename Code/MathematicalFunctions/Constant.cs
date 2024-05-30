using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    internal class Constant : IFunction
    {
        private double _value;

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Constant(double value)
        {
            _value = value;
        }

        public double Evaluate(double x)
        {
            return _value;
        }
    }
}