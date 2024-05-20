using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class Exp : IFunction
    {
        private readonly double _a;
        public Exp(double a)
        {
            _a = a;
        }

        public double Evaluate(double x)
        {
            return Math.Exp(_a * x);
        }
    }
}
