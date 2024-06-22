using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;
using QuantitativeLibrary.Time;

namespace Bootstrapping.YieldComputer
{
    public class YieldComputerUsingNewtonSolver : IYieldComputer
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public YieldComputerUsingNewtonSolver(Parameters parameters)
        {
            _parameters = parameters;
        }

        private static double GetYield(double result, double deltaTotal, Parameters parameters)
        {
            switch (parameters.VariableChoice)
            {
                case VariableChoice.Discount:
                    return -Math.Log(result) / deltaTotal;
                case VariableChoice.Yield:
                    return result;
                default:
                    throw new ArgumentException("Unknown variable choice. Found: " + parameters.VariableChoice);
            }
        }

        private static double GetDiscount(double result, double deltaTotal, Parameters parameters)
        {
            switch (parameters.VariableChoice)
            {
                case VariableChoice.Discount:
                    return result;
                case VariableChoice.Yield:
                    return Math.Exp(-result * deltaTotal);
                default:
                    throw new ArgumentException("Unknown variable choice. Found: " + parameters.VariableChoice);
            }
        }

        public List<double> Compute(Dictionary<Period, double> interpolatedSwapRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;
            var newtonSolverParameters = _parameters.NewtonSolverParameters;
            var solveRoot = newtonSolverParameters.SolveRoot;
            var target = newtonSolverParameters.Target;
            var firstGuess = newtonSolverParameters.FirstGuess;
            var tolerance = newtonSolverParameters.Tolerance;

            List<double> yields = new List<double>();
            List<double> zeroCoupons = new List<double>();

            double zeroCoupon;
            double yield;

            Dictionary<Period, RFunction> zeroCouponDictionary = new Dictionary<Period, RFunction>();

            var f = Utilities.Duration(periodicity, pricingDate, counter);
            double deltaTotal = f;
            var firstSwap = interpolatedSwapRates[periodicity];
            Period f1 = new Period(periodicity.NbUnit, periodicity.Unit);
            var swapFunc = SwapFunc.SwapValue(zeroCouponDictionary, interpolatedSwapRates[f1], _parameters);
            NewtonSolver solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
            NewtonResult result = solver.Solve();
            zeroCoupon = result.Solution;
            yield = -Math.Log(zeroCoupon) / deltaTotal;
            zeroCoupons.Add(zeroCoupon);
            yields.Add(yield);

            zeroCouponDictionary.Add(periodicity, zeroCoupon);

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(periodicity);

            var lastMaturity = Utilities.ConvertPeriodToMonths(interpolatedSwapRates.Keys.Last());
            var j = 2;
            while (j * periodicity.NbUnit <= lastMaturity.NbUnit)
            {
                datePrevious = date;
                date = date.Advance(periodicity);
                f = counter.YearFraction(datePrevious, date);

                Period fi = new Period(j * periodicity.NbUnit, periodicity.Unit);
                swapFunc = SwapFunc.SwapValue(zeroCouponDictionary, interpolatedSwapRates[fi], _parameters);
                solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
                result = solver.Solve();

                deltaTotal += f;
                zeroCoupon = GetDiscount(result.Solution, deltaTotal, Parameters);
                yield = GetYield(result.Solution, deltaTotal, Parameters);

                zeroCoupons.Add(zeroCoupon);
                yields.Add(yield);
                zeroCouponDictionary.Add(fi, zeroCoupon);

                j++;
            }

            return yields;
        }
    }
}
