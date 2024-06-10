using MathematicalFunctions;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;
using QuantitativeLibrary.Maths.Functions;

namespace Bootstrapping
{
    public class Discount : RFunction, IDiscountCurve
    {
        private RFunction _yieldF;
        public RFunction YieldF
        {
            get { return _yieldF; }
            set { _yieldF = value; }
        }

        public Discount(RFunction yieldF)
        {
            _yieldF = yieldF;
        }

        public override double Evaluate(double x)
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
