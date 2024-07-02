using Bootstrapping.CurveParameters;
using Bootstrapping.YieldComputer;
using MathematicalFunctions;
using QuantitativeLibrary.Time;
using Bootstrapping.InterpolationMethods;

namespace Bootstrapping
{
    public class Algorithm
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public Algorithm(Parameters parameters)
        {
            _parameters = parameters;
        }

        private IYieldComputerUsingSwaps GetYieldComputer()
        {
            switch (_parameters.InterpolationChoice)
            {
                case InterpolationChoice.UsingDirectSolving:
                    return new YieldComputerUsingDirectSolving(_parameters);

                case InterpolationChoice.UsingNewtonSolver:
                    return new YieldComputerUsingNewtonSolver(_parameters);

                default:
                    throw new ArgumentException("Unknown interpolation choice. Found: " + _parameters.InterpolationChoice);
            }
        }

        private Interpolator GetInterpolationMethod()
        {
            switch (_parameters.InterpolationMethod)
            {
                case InterpolationMethod.LinearOnYield:
                    return new LinearOnYield(_parameters);

                case InterpolationMethod.LinearOnYieldLog:
                    return new LinearOnYieldLog(_parameters);

                default:
                    throw new ArgumentException("Unknown interpolation choice. Found: " + _parameters.InterpolationMethod);
            }
        }

        private Dictionary<Period, double> GetSwapRates(Dictionary<Period, double> swapRates)
        {
            switch (_parameters.DataChoice)
            {
                case DataChoice.InterpolatedData:
                    return InterpolateSwapRates(swapRates);
                case DataChoice.RawData:
                    return swapRates.ToDictionary(pairValue => Utilities.ConvertPeriodToMonths(pairValue.Key), key => key.Value);
                default:
                    throw new ArgumentException("Unknown data format choice. Found: " + _parameters.DataChoice);
            }
        }

        public Discount Curve(Dictionary<Period, double> swapRates)
        {
            // Get the swap rates
            var newSwapRates = GetSwapRates(swapRates);

            // Compute and store the ZC prices and yield curve values for the given maturities
            var yieldComputer = GetYieldComputer();
            var yields = yieldComputer.Compute(newSwapRates);

            // Define the function: t -> y(0, t)
            var interpolator = GetInterpolationMethod();
            var yield = interpolator.Compute(yields);

            var pricingDate = _parameters.PricingDate;
            var dayCounter = _parameters.DayCounter;
            return new Discount(pricingDate, dayCounter, yield);
        }

        public Discount Curve(Dictionary<Date, double> futureRates)
        {
            // Compute and store the ZC prices and yield curve values for the given maturities
            var yieldComputer = new YieldComputerUsingFuturesMain(_parameters);
            var yields = yieldComputer.Compute(futureRates);

            // Define the function: t -> y(0, t)
            var interpolator = new LinearOnYield(_parameters);
            var yield = interpolator.Compute(yields);

            var pricingDate = _parameters.PricingDate;
            var dayCounter = _parameters.DayCounter;
            return new Discount(pricingDate, dayCounter, yield);
        }

        private Dictionary<Period, double> InterpolateSwapRates(Dictionary<Period, double> swapRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

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

            return Utilities.InterpolateSwapRate(SwapInt, swapRates, _parameters);
        }
    }
}
