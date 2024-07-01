using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping.InterpolationMethods
{
    public class LinearOnYield : Interpolator
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public LinearOnYield(Parameters parameters)
        {
            _parameters = parameters;
        }
        public RFunction Compute(List<double> yields)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(periodicity);

            var fN = counter.YearFraction(datePrevious, date);

            YieldF.AddInterval(0, yields[0], fN, yields[0]);

            for (int i = 1; i < yields.Count; i++)
            {
                datePrevious = date;
                date = date.Advance(periodicity);

                double x1 = counter.YearFraction(pricingDate, datePrevious);
                double y1 = yields[i - 1];
                double x2 = counter.YearFraction(pricingDate, date);
                double y2 = yields[i];
                YieldF.AddInterval(x1, y1, x2, y2);
            }
            double xFinal = counter.YearFraction(pricingDate, date);

            YieldF.AddInterval(xFinal, yields.Last(), double.PositiveInfinity, yields.Last());

            return YieldF;
        }

        public RFunction Compute(Dictionary<Date, double> yields)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var x1 = counter.YearFraction(pricingDate, yields.Keys.First());
            var y1 = yields.Values.First();
            YieldF.AddInterval(0, y1, x1, y1);

            foreach (var yield in yields)
            {
                var x2 = counter.YearFraction(pricingDate, yield.Key);
                var y2 = yield.Value;

                YieldF.AddInterval(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            YieldF.AddInterval(x1, y1, double.PositiveInfinity, y1);

            return YieldF;
        }
    }
    
}
