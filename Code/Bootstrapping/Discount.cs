using MathematicalFunctions;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

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

        public double At(Date startDate, Date endDate)
        {
            var counter = new DayCounter(DayConvention.ACT365);
            double x = counter.YearFraction(startDate, endDate);
            return Evaluate(x);
        }

        public override string ToString()
        {
            return "Discount curve: Exp[-t * " + _yieldF.ToString() + "]";
        }
    }
}
