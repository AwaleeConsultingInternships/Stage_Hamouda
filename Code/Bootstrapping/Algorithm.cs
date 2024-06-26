using Bootstrapping.CurveParameters;
using Bootstrapping.YieldComputer;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

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

                default:
                    throw new ArgumentException("Unknown interpolation choice. Found: " + _parameters.InterpolationChoice);
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
            var yield = ComputeYield(yields);

            var pricingDate = _parameters.PricingDate;
            return new Discount(pricingDate, yield);
        }

        public RFunction YieldCurve(Dictionary<Period, double> swapRates)
        {
            // Get the swap rates
            var newSwapRates = GetSwapRates(swapRates);

            // Compute and store the ZC prices and yield curve values for the given maturities
            var yieldComputer = GetYieldComputer();
            var yields = yieldComputer.Compute(newSwapRates);

            // Define the function: t -> y(0, t)
            var yield = ComputeYield(yields);

            return yield;
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

        private PiecewiseLinear ComputeYield(List<double> yields)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;

            PiecewiseLinear YieldF = new PiecewiseLinear();

            var datePreviousN = pricingDate;
            var dateN = pricingDate.Advance(periodicity);

            var fN = counter.YearFraction(datePreviousN, dateN);

            YieldF.AddInterval(0, yields[0], fN, yields[0]);

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
