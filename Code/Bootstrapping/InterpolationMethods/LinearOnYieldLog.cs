using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping.InterpolationMethods
{
    public class LinearOnYieldLog : Interpolator
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public LinearOnYieldLog(Parameters parameters)
        {
            _parameters = parameters;
        }
        /*public RFunction Compute(List<double> yields)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(periodicity);

            var f = counter.YearFraction(datePrevious, date);

            YieldF.AddInterval(0, Math.Log(yields[0]), f, Math.Log(yields[0]));

            for (int i = 1; i < yields.Count; i++)
            {
                datePrevious = date;
                date = date.Advance(periodicity);

                double x1 = counter.YearFraction(pricingDate, datePrevious);
                double y1 = Math.Log(yields[i - 1]);
                double x2 = counter.YearFraction(pricingDate, date);
                double y2 = Math.Log(yields[i]);
                YieldF.AddInterval(x1, y1, x2, y2);
            }
            double xFinal = counter.YearFraction(pricingDate, date);

            YieldF.AddInterval(xFinal, Math.Log(yields.Last()), double.PositiveInfinity, Math.Log(yields.Last()));
            var YieldFinal = new Composite(new Exp(1), YieldF);

            return YieldFinal;
        }*/

        public RFunction Compute(Dictionary<Date, double> yields)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var x1 = counter.YearFraction(pricingDate, yields.Keys.First());
            var y1 = Math.Log(yields.Values.First());
            YieldF.AddInterval(0, y1, x1, y1);

            foreach (var yield in yields)
            {
                var x2 = counter.YearFraction(pricingDate, yield.Key);
                var y2 = Math.Log(yield.Value);

                YieldF.AddInterval(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            YieldF.AddInterval(x1, y1, double.PositiveInfinity, y1);
            var YieldFinal = new Composite(new Exp(1), YieldF);

            return YieldFinal;
        }
    }

}
