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

        private IYieldComputer GetYieldComputer()
        {
            switch (_parameters.InterpolationChoice)
            {
                case InterpolationChoice.UsingDirectSolving:
                    return new YieldComputerUsingDirectSolving(_parameters);

                case InterpolationChoice.UsingNewtonSolver:
                    return new YieldComputerUsingNewtonSolver(_parameters);

                case InterpolationChoice.UsingFuturesMain:
                    return new YieldComputerUsingFuturesMain(_parameters);

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

        private Dictionary<Date, double> GetSwapRates(Dictionary<Date, double> Rates)
        {
            switch (_parameters.DataChoice)
            {
                case DataChoice.InterpolatedData:
                    return InterpolateSwapRates(Rates);
                case DataChoice.RawData:
                    return Rates;
                default:
                    throw new ArgumentException("Unknown data format choice. Found: " + _parameters.DataChoice);
            }
        }

        public Discount Curve(Dictionary<Date, double> Rates)
        {
            // Get the swap rates
            var newRates = GetSwapRates(Rates);

            // Compute and store the ZC prices and yield curve values for the given maturities
            var yieldComputer = GetYieldComputer();
            var yields = yieldComputer.Compute(newRates);

            // Define the function: t -> y(0, t)
            var interpolator = GetInterpolationMethod();
            var yield = interpolator.Compute(yields);

            var pricingDate = _parameters.PricingDate;
            var dayCounter = _parameters.DayCounter;
            return new Discount(pricingDate, dayCounter, yield);
        }

        private Dictionary<Date, double> InterpolateSwapRates(Dictionary<Date, double> swapRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            PiecewiseLinear SwapInt = new PiecewiseLinear();

            var maturities = swapRates.Keys.ToArray();
            var x00 = Utilities.Duration(periodicity, pricingDate, counter);
            var x0 = counter.YearFraction(pricingDate, maturities[0]);
            var y0 = swapRates[maturities[0]];
            if (x00 < x0)
            {
                SwapInt.AddInterval(x00, y0, x0, y0);
            }

            for (int i = 1; i < swapRates.Keys.Count; i++)
            {
                Date d1 = maturities[i - 1];
                double x1 = counter.YearFraction(pricingDate, d1);
                double y1 = swapRates[d1];
                Date d2 = maturities[i];
                double x2 = counter.YearFraction(pricingDate, d2);
                double y2 = swapRates[d2];
                SwapInt.AddInterval(x1, y1, x2, y2);
            }
            Date lastDate = maturities[maturities.Length - 1];
            double lastX = counter.YearFraction(pricingDate, lastDate);
            SwapInt.AddInterval(lastX, swapRates[lastDate], double.PositiveInfinity, swapRates[lastDate]);

            return Utilities.InterpolateSwapRate(SwapInt, swapRates, _parameters);
        }
    }
}
