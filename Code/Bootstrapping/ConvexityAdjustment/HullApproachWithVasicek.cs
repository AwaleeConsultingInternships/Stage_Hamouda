using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping.ConvexityAdjustment
{
    public class HullApproachWithVasicek : CAModel
    {
        private double _sigma;
        private Parameters _parameters;
        private double _a;

        public HullApproachWithVasicek(Parameters parameters, double sigma = 0.2, double a = 0.2)
        {
            _parameters = parameters;
            _sigma = sigma;
            _a = a;
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
                var B12 = (1 - Math.Exp(_a * (x1 - x2))) / _a;
                var B01 = (1 - Math.Exp(-1 * _a * x1)) / _a;
                var CA = B12 * (B12*(1 - Math.Exp(-1 * _a * x1)) + 2 * _a * Math.Pow(B01, 2)) * Math.Pow(_sigma, 2) / ((x2 - x1) * 4 * _a);
                forwardRates[key] = futureRates[key] - CA;
            }
            return forwardRates;
        }
    }
}
