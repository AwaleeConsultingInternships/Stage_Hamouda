using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;
using QuantitativeLibrary.Time;

namespace Bootstrapping.YieldComputer
{
    public class YieldComputerUsingNewtonSolver : IYieldComputerUsingSwaps
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

        public List<double> Compute(Dictionary<Period, double> swapRates)
        {
            var newSwapRates = new Dictionary<Period, double>();
            foreach (var pair in swapRates)
            {
                newSwapRates.Add(Utilities.ConvertPeriodToMonths(pair.Key), pair.Value);
            }
            swapRates = newSwapRates;

            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;
            var newtonSolverParameters = _parameters.NewtonSolverParameters;
            var target = newtonSolverParameters.Target;
            var firstGuess = newtonSolverParameters.FirstGuess;
            var tolerance = newtonSolverParameters.Tolerance;

            List<double> yields = new List<double>();
            List<double> ZC = new List<double>();

            double P;
            double y;

            Dictionary<Period, RFunction> ZCDict = new Dictionary<Period, RFunction>();

            var swapLeft = swapRates.First();
            int x1 = 0;
            double y1 = 1;
            foreach (var swapRate in swapRates)
            {
                var swapRight = swapRate;
                var x3 = swapRight.Key.NbUnit;
                for (int freq = x1 + periodicity.NbUnit; freq < x3; freq += periodicity.NbUnit)
                {
                    var x2 = freq;
                    var slope = (double)(x2 - x1) / (x3 - x1);
                    var origin = y1 * (x3 - x2) / (x3 - x1);
                    var PFunction = new AffineFunction(origin, slope);
                    ZCDict.Add(new Period(freq, periodicity.Unit), PFunction);
                }
                var swapFunc = SwapFunc.SwapValue(ZCDict, swapRight.Value, _parameters);
                var solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
                var result = solver.Solve();

                var deltaTotal = Utilities.Duration(swapRate.Key, pricingDate, counter);
                P = IYieldComputer.GetDiscount(result.Solution, deltaTotal, Parameters);
                ZCDict.Add(swapRight.Key, P);
                for (int freq = x1 + periodicity.NbUnit; freq < x3; freq += periodicity.NbUnit)
                {
                    var interP = new Period(freq, periodicity.Unit);
                    ZCDict[interP] = ZCDict[interP].Evaluate(P);
                }
                swapLeft = swapRight;
                x1 = x3;
                y1 = ZCDict[swapLeft.Key].Evaluate(1);
            }

            foreach (var ZCfinal in ZCDict)
            {
                P = ZCfinal.Value.Evaluate(1);
                ZC.Add(P);
                var deltaTotal = Utilities.Duration(ZCfinal.Key, pricingDate, counter);
                y = -Math.Log(P) / deltaTotal;
                yields.Add(y);
            }

            return yields;
        }
    }
}
