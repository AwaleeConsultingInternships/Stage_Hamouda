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

        public Dictionary<Date, double> Compute(Dictionary<Date, double> interpolatedSwapRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            Dictionary<Date, double> yields = new Dictionary<Date, double>();
            List<double> ZC = new List<double>();

            double P;
            double y;

            var delta = counter.YearFraction(pricingDate, interpolatedSwapRates.Keys.First());
            double delta_total = delta;
            double Q = 0;
            var firstSwap = interpolatedSwapRates.Values.First();
            P = 1 / (1 + firstSwap * delta);
            y = -Math.Log(P) / delta_total;
            ZC.Add(P);
            yields.Add(interpolatedSwapRates.Keys.First(), y);

            var datePrevious = interpolatedSwapRates.Keys.First();
            foreach (var swap in interpolatedSwapRates.Skip(1))
            {
                Date date = swap.Key;
                Q += P * delta;
                delta = counter.YearFraction(datePrevious, date);
                P = (1 - swap.Value * Q) / (1 + swap.Value * delta);
                delta_total += delta;

                datePrevious = date;
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(date, y);
            }

            return yields;
        }
    }
}
