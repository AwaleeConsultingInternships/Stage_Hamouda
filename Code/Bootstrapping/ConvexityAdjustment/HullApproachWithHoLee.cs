using QuantitativeLibrary.Time;
using Bootstrapping.CurveParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace Bootstrapping.ConvexityAdjustment
{
    public class HullApproachWithHoLee : CAModel
    {
        private double _sigma;
        private Parameters _parameters;

        public HullApproachWithHoLee(Parameters parameters, double sigma = 0.2)
        {
            _parameters = parameters;
            _sigma = sigma;
        }

        public Dictionary<Date, double> AdjustConvexity(Dictionary<Date, double> futureRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            Dictionary<Date, double> forwardRates = new Dictionary<Date, double>();

            foreach (var key in futureRates.Keys.ToList())
            {
                var T1 = key;
                var T2 = Utilities.GetFutureMaturity(T1);
                var x1 = counter.YearFraction(pricingDate, T1);
                var x2 = counter.YearFraction(pricingDate, T2);
                forwardRates[key] = futureRates[key] - _sigma * x1 * x2;
            }
            return forwardRates;
        }
    }
}
