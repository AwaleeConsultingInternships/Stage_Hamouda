using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    internal class Constant : IFunction
    {
        private readonly double _value;
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