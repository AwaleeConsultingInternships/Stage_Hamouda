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
        public RFunction Compute(List<double> yields)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var datePreviousN = pricingDate;
            var dateN = pricingDate.Advance(periodicity);

            var fN = counter.YearFraction(datePreviousN, dateN);

            YieldF.AddInterval(0, Math.Log(yields[0]), fN, Math.Log(yields[0]));

            for (int i = 1; i < yields.Count; i++)
            {
                datePreviousN = dateN;
                dateN = dateN.Advance(periodicity);

                double x1 = counter.YearFraction(pricingDate, datePreviousN);
                double y1 = Math.Log(yields[i - 1]);
                double x2 = counter.YearFraction(pricingDate, dateN);
                double y2 = Math.Log(yields[i]);
                YieldF.AddInterval(x1, y1, x2, y2);
            }
            double xFinal = counter.YearFraction(pricingDate, dateN);

            YieldF.AddInterval(xFinal, Math.Log(yields.Last()), double.PositiveInfinity, Math.Log(yields.Last()));
            var YieldFinal = new Composite(new Exp(1), YieldF);

            return YieldFinal;
        }
    }

}
