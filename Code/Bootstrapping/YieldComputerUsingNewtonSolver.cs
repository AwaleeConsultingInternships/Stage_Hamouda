using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;
using QuantitativeLibrary.Time;

namespace Bootstrapping
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

        public List<double> Compute(Period lastMaturity, Dictionary<Period, double> interpolatedSwapRates)
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
            List<double> ZC = new List<double>();

            double P;
            double y;

            Dictionary<Period, RFunction> ZCDict = new Dictionary<Period, RFunction>();

            var f = Utilities.Duration(periodicity, pricingDate, counter);
            double delta_total = f;
            var firstSwap = interpolatedSwapRates[periodicity];
            RFunction swapFunc = new AffineFunction(1, -1 - firstSwap * f);
            //var sswapFunc = new Composite(new AffineFunction(1, -1 - firstSwap * f), new Exp(-f));
            NewtonSolver solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
            NewtonResult result = solver.Solve();
            P = result.Solution;
            y = -Math.Log(P) / delta_total;
            ZC.Add(P);
            yields.Add(y);

            ZCDict.Add(periodicity, new ConstantFunction(P));

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(periodicity);

            var j = 2;
            while (j * periodicity.NbUnit <= lastMaturity.NbUnit)
            {
                datePrevious = date;
                date = date.Advance(periodicity);
                f = counter.YearFraction(datePrevious, date);

                Period fi = new Period(j * periodicity.NbUnit, periodicity.Unit);
                swapFunc = SwapFunc.SwapValue(ZCDict, interpolatedSwapRates[fi], _parameters);
                solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
                result = solver.Solve();
                P = result.Solution;

                delta_total += f;
                y = -Math.Log(P) / delta_total;

                ZC.Add(P);
                yields.Add(y);
                ZCDict.Add(fi, new ConstantFunction(P));

                j++;
            }

            return yields;
        }
    }
}
