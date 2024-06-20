using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping
{
    public class YieldComputerUsingRawData : IYieldComputer
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public YieldComputerUsingRawData(Parameters parameters)
        {
            _parameters = parameters;
        }

        public List<double> Compute(Period lastMaturity, Dictionary<Period, double> swapRates)
        {
            var newSwapRates = new Dictionary<Period, double>();
            foreach (var pair in swapRates)
            {
                newSwapRates.Add(Utilities.ConvertYearsToMonths(pair.Key), pair.Value);
            }
            swapRates = newSwapRates;
            
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

            // Assuming that periodicity <= First Maturity in swapRates
            var f = Utilities.Duration(periodicity, pricingDate, counter);
            double delta_total = f;
            var firstSwap = swapRates[periodicity];
            RFunction swapFunc = new AffineFunction(1, -1 - firstSwap * f);

            NewtonSolver solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
            NewtonResult result = solver.Solve();
            P = result.Solution;

            ZCDict.Add(periodicity, new ConstantFunction(P));

            var swapLeft = swapRates.First();
            foreach (var swapRate in swapRates.Skip(1))
            {
                var swapRight = swapRate;
                for (int freq = swapLeft.Key.NbUnit + periodicity.NbUnit;  freq < swapRight.Key.NbUnit; freq += periodicity.NbUnit)
                {
                    var x1 = swapLeft.Key.NbUnit;
                    var x2 = freq;
                    var x3 = swapRight.Key.NbUnit;
                    var y1 = ZCDict[swapLeft.Key].Evaluate(1);
                    var slope = (double)(x2 - x1) / (x3 - x1);
                    var origin = y1 * (x3 - x2) / (x3 - x1);
                    var PFunction = new AffineFunction(origin, slope);
                    ZCDict.Add(new Period(freq, periodicity.Unit), PFunction);
                }
                swapFunc = SwapFunc.SwapValue(ZCDict, swapRight.Value, _parameters);
                solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
                result = solver.Solve();
                P = result.Solution;
                ZCDict.Add(swapRight.Key, new ConstantFunction(P));
                for (int freq = swapLeft.Key.NbUnit + periodicity.NbUnit; freq < swapRight.Key.NbUnit; freq += periodicity.NbUnit)
                {
                    var interP = new Period(freq, periodicity.Unit);
                    ZCDict[interP] = new ConstantFunction(ZCDict[interP].Evaluate(P));
                }
                swapLeft = swapRight;
            }
            
            foreach (var ZCfinal in ZCDict)
            {
                P = ZCfinal.Value.Evaluate(1);
                ZC.Add(P);
                delta_total = Utilities.Duration(ZCfinal.Key, pricingDate, counter);
                y = -Math.Log(P) / delta_total;
                yields.Add(y);
            }

            return yields;
        }
    }
}
