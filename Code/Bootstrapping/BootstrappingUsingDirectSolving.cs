using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class BootstrappingUsingDirectSolving : BootstrappingAlgorithm
    {
        public BootstrappingUsingDirectSolving(BootstrappingParameters bootstrappedParameters)
            : base(bootstrappedParameters)
        {
        }

        public override List<double> ComputeYieldsAtMaturities(Period lastMaturity, Dictionary<Period, double> interpolatedSwapRates)
        {
            var pricingDate = _bootstrappedParameters.PricingDate;
            var counter = _bootstrappedParameters.DayCounter;
            var periodicity = _bootstrappedParameters.Periodicity;
            var solveRoot = _bootstrappedParameters.SolveRoot;
            var target = _bootstrappedParameters.Target;
            var firstGuess = _bootstrappedParameters.FirstGuess;
            var tolerance = _bootstrappedParameters.Tolerance;

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
