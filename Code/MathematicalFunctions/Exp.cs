using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class Exp : IFunction
    {
        private double _a;

        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

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
