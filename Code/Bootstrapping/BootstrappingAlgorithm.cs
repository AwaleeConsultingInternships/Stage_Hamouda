using MathematicalFunctions;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public abstract class BootstrappingAlgorithm
    {
        public BootstrappingParameters _bootstrappedParameters;
        public BootstrappingParameters BootstrappedParameters
        {
            get { return _bootstrappedParameters; }
            set { _bootstrappedParameters = value; }
        }

        public BootstrappingAlgorithm(BootstrappingParameters bootstrappedParameters)
        {
            _bootstrappedParameters = bootstrappedParameters;
        }

        public Discount Curve(Dictionary<Period, double> swapRates)
        {
            var pricingDate = _bootstrappedParameters.PricingDate;
            var counter = _bootstrappedParameters.DayCounter;
            var periodicity = _bootstrappedParameters.Periodicity;

            // Interpoalte the missing values for the swap rates
            var interpolatedSwapRates = InterpolateSwapRates(swapRates);

            // Compute and store the ZC prices and yield curve values for the given maturities
            var lastMaturity = Utilities.ConvertYearsToMonths(swapRates.Keys.Last());
            var yields = ComputeYieldsAtMaturities(lastMaturity, interpolatedSwapRates);

            // Define the function: t -> y(0, t)
            var YieldF = ComputeYields(yields);

            return new Discount(YieldF);
        }

        public abstract List<double> ComputeYieldsAtMaturities(Period lastMaturity, Dictionary<Period, double> interpolatedSwapRates);

        private Dictionary<Period, double> InterpolateSwapRates(Dictionary<Period, double> swapRates)
        {
            var pricingDate = _bootstrappedParameters.PricingDate;
            var counter = _bootstrappedParameters.DayCounter;
            var periodicity = _bootstrappedParameters.Periodicity;

            PiecewiseLinear SwapInt = new PiecewiseLinear();

            var maturities = swapRates.Keys.ToArray();
            var x00 = Utilities.Duration(periodicity, pricingDate, counter);
            var x0 = Utilities.Duration(maturities[0], pricingDate, counter);
            var y0 = swapRates[maturities[0]];
            if (x00 < x0)
            {
                SwapInt.AddInterval(x00, y0, x0, y0);
            }

            for (int i = 1; i < swapRates.Keys.Count; i++)
            {
                Period p1 = maturities[i - 1];
                double x1 = Utilities.Duration(p1, pricingDate, counter);
                double y1 = swapRates[p1];
                Period p2 = maturities[i];
                double x2 = Utilities.Duration(p2, pricingDate, counter);
                double y2 = swapRates[p2];
                SwapInt.AddInterval(x1, y1, x2, y2);
            }
            Period lastPeriod = maturities[maturities.Length - 1];
            double lastX = Utilities.Duration(lastPeriod, pricingDate, counter);
            SwapInt.AddInterval(lastX, swapRates[lastPeriod], double.PositiveInfinity, swapRates[lastPeriod]);

            return Utilities.InterpolateSwapRate(SwapInt, swapRates, _bootstrappedParameters);
        }

        private PiecewiseLinear ComputeYields(List<double> yields)
        {
            var pricingDate = _bootstrappedParameters.PricingDate;
            var counter = _bootstrappedParameters.DayCounter;
            var periodicity = _bootstrappedParameters.Periodicity;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var datePreviousN = pricingDate;
            var dateN = pricingDate.Advance(periodicity);

            var fN = counter.YearFraction(datePreviousN, dateN);

            YieldF.AddInterval(0, 0, fN, yields[0]);

            for (int i = 1; i < yields.Count; i++)
            {
                datePreviousN = dateN;
                dateN = dateN.Advance(periodicity);

                double x1 = counter.YearFraction(pricingDate, datePreviousN);
                double y1 = yields[i - 1];
                double x2 = counter.YearFraction(pricingDate, dateN);
                double y2 = yields[i];
                YieldF.AddInterval(x1, y1, x2, y2);
            }
            double xFinal = counter.YearFraction(pricingDate, dateN);

            YieldF.AddInterval(xFinal, yields.Last(), double.PositiveInfinity, yields.Last());

            return YieldF;
        }
    }
}
