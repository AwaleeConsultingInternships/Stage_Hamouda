using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping.ConvexityAdjustment
{
    public class MartingaleApproachWithHoLee : CAModel
    {
        private double _sigmaR;
        private double _sigmaF;
        private double _rho;
        private Parameters _parameters;

        public MartingaleApproachWithHoLee(Parameters parameters, double sigmaR = 0.2, double sigmaF = 0.2, double rho = 0.5)
        {
            _parameters = parameters;
            _sigmaR = sigmaR;
            _sigmaF = sigmaF;
            _rho = rho;
        }

        public Dictionary<Date, double> AdjustConvexity(Dictionary<Date, double> futureRates)
        {
            Dictionary<Date, double> forwardRates = new Dictionary<Date, double>();
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;

            foreach (var key in futureRates.Keys.ToList())
            {
                var T1 = key;
                var T2 = Utilities.GetFutureMaturity(T1);
                var x1 = counter.YearFraction(pricingDate, T1);
                var x2 = counter.YearFraction(pricingDate, T2);
                var CA = Math.Exp(_sigmaF * _sigmaR * _rho * x1 * (x1 - 2 * x2) / 2);
                forwardRates[key] = futureRates[key] * CA;
            }
            return forwardRates;
        }
    }
}