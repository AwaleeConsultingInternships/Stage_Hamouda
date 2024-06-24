using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Time;

namespace Bootstrapping.YieldComputer
{
    public class YieldComputerUsingDirectSolving : IYieldComputer
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public YieldComputerUsingDirectSolving(Parameters parameters)
        {
            _parameters = parameters;
        }

        public List<double> Compute(Dictionary<Period, double> interpolatedSwapRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            List<double> yields = new List<double>();
            List<double> ZC = new List<double>();

            double P;
            double y;

            var f = Utilities.Duration(periodicity, pricingDate, counter);
            double delta_total = f;
            double Q = 0;
            var firstSwap = interpolatedSwapRates[periodicity];
            P = 1 / (1 + firstSwap * f);
            y = -Math.Log(P) / delta_total;
            ZC.Add(P);
            yields.Add(y);

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(periodicity);

            var lastMaturity = Utilities.ConvertPeriodToMonths(interpolatedSwapRates.Keys.Last());
            var j = 2;
            while (j * periodicity.NbUnit <= lastMaturity.NbUnit)
            {
                datePrevious = date;
                date = date.Advance(periodicity);
                Q += P * f;
                f = counter.YearFraction(datePrevious, date);
                Period fi = new Period(j * periodicity.NbUnit, periodicity.Unit);
                P = (1 - interpolatedSwapRates[fi] * Q) / (1 + interpolatedSwapRates[fi] * f);
                delta_total += f;
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);

                j++;
            }

            return yields;
        }
    }
}
