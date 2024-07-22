using Bootstrapping.CurveParameters;
using Bootstrapping.StrippingInstruments;
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

        public Dictionary<Date, double> Compute(Dictionary<Date, double> swapRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;
            var newtonSolverParameters = _parameters.NewtonSolverParameters;
            var target = newtonSolverParameters.Target;
            var firstGuess = newtonSolverParameters.FirstGuess;
            var tolerance = newtonSolverParameters.Tolerance;

            Dictionary<Date, double> yields = new Dictionary<Date, double>();
            List<double> ZC = new List<double>();

            double P;
            double y;

            Dictionary<Date, RFunction> ZCDict = new Dictionary<Date, RFunction>();

            var swapLeft = swapRates.First();
            double x1 = 0;
            double y1 = 1; 
            Date leftDate = pricingDate;

            foreach (var swapRate in swapRates)
            {
                var swapRight = swapRate;
                var x3 = counter.YearFraction(pricingDate, swapRight.Key);

                for (var intermDate = leftDate.Advance(periodicity); intermDate < swapRight.Key; intermDate = intermDate.Advance(periodicity))
                {
                    var x2 = counter.YearFraction(pricingDate, intermDate);
                    var slope = (x2 - x1) / (x3 - x1);
                    var origin = y1 * (x3 - x2) / (x3 - x1);
                    var PFunction = new AffineFunction(origin, slope); 

                    ZCDict.Add(intermDate, PFunction);
                }
                var swapFunc = SwapFunc.SwapValue(ZCDict, swapRight, _parameters);
                var solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
                var result = solver.Solve();

                var deltaTotal = counter.YearFraction(pricingDate, swapRate.Key);
                P = IYieldComputer.GetDiscount(result.Solution, deltaTotal, Parameters);
                ZCDict.Add(swapRight.Key, P);

                for (var intermDate = leftDate.Advance(periodicity); intermDate < swapRight.Key; intermDate = intermDate.Advance(periodicity))
                {
                    ZCDict[intermDate] = ZCDict[intermDate].Evaluate(P);
                }

                swapLeft = swapRight;
                leftDate = swapLeft.Key;
                x1 = x3;
                y1 = ZCDict[swapLeft.Key].Evaluate(1);
            }

            foreach (var ZCfinal in ZCDict)
            {
                P = ZCfinal.Value.Evaluate(1);
                ZC.Add(P);
                var deltaTotal = counter.YearFraction(pricingDate, ZCfinal.Key);
                y = -Math.Log(P) / deltaTotal;
                yields.Add(ZCfinal.Key, y);
            }

            return yields;
        }
    }
}
