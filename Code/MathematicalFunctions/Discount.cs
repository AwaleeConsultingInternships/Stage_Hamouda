using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class Discount : IFunction
    {
        private IFunction _yieldF;
        public IFunction YieldF
        {
            get { return _yieldF; }
            set { _yieldF = value; }
        }

        public Discount(IFunction yieldF)
        {
            _yieldF = yieldF;
        }

        public double Evaluate(double x)
        {
            double yield = _yieldF.Evaluate(x);
            return Math.Exp(-yield * x);
        }
    }
}
