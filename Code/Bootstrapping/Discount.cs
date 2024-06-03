using MathematicalFunctions;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping
{
    public class Discount : IFunction, IDiscountCurve
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

        public double At(Date date)
        {
            throw new NotImplementedException();
        }
    }
}
