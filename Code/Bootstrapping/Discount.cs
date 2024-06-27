using QuantitativeLibrary.Time;
using QuantitativeLibrary.Maths.Functions;

namespace Bootstrapping
{
    public class Discount : RFunction, IDiscount
    {
        private Date _pricingDate;
        public Date PricingDate
        {
            get { return _pricingDate; }
            set { _pricingDate = value; }
        }

        private DayCounter _dayCounter;
        public DayCounter DayCounter
        {
            get { return _dayCounter; }
            set { _dayCounter = value; }
        }

        private RFunction _yieldF;
        public RFunction YieldF
        {
            get { return _yieldF; }
            set { _yieldF = value; }
        }

        public Discount(Date pricingDate, DayCounter dayCounter, RFunction yieldF)
        {
            _pricingDate = pricingDate;
            _dayCounter = dayCounter;
            _yieldF = yieldF;
        }

        public override double Evaluate(double x)
        {
            double yield = _yieldF.Evaluate(x);
            return Math.Exp(-yield * x);
        }

        public double At(Date date)
        {
            double x = _dayCounter.YearFraction(_pricingDate, date);
            return Evaluate(x);
        }

        public double YieldAt(Date date)
        {
            double x = _dayCounter.YearFraction(_pricingDate, date);
            return _yieldF.Evaluate(x);
        }

        public double ZcYieldAt(Date date)
        {
            var ratio = 1 / At(date);
            var time = _dayCounter.YearFraction(_pricingDate, date);
            return (ratio - 1) / time;
        }

        public double ForwardAt(Period period, Date date)
        {
            var endDate = date.Advance(period);
            var ratio = At(endDate) / At(date);
            var start = _dayCounter.YearFraction(_pricingDate, date);
            var end = _dayCounter.YearFraction(_pricingDate, endDate);
            var time = end - start;
            return (ratio - 1) / time;
        }

        public override string ToString()
        {
            return "Discount curve: Exp[-t * " + _yieldF.ToString() + "]";
        }
    }
}
