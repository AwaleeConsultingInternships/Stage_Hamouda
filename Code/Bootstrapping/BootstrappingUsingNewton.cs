using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class BootstrappingUsingNewton : BootstrappingAlgorithm
    {
        public BootstrappingUsingNewton(BootstrappingParameters bootstrappedParameters)
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

            Dictionary<Period, double> ZCDict = new Dictionary<Period, double>();

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

            ZCDict.Add(periodicity, P);

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(periodicity);

            var j = 2;
            while (j * periodicity.NbUnit <= lastMaturity.NbUnit)
            {
                datePrevious = date;
                date = date.Advance(periodicity);
                f = counter.YearFraction(datePrevious, date);

                Period fi = new Period(j * periodicity.NbUnit, periodicity.Unit);
                swapFunc = SwapFunc.SwapValue(ZCDict, interpolatedSwapRates[fi], _bootstrappedParameters);
                solver = NewtonSolver.CreateWithAbsolutePrecision(target, swapFunc, swapFunc.FirstDerivative, firstGuess, tolerance);
                result = solver.Solve();
                P = result.Solution;

                delta_total += f;
                y = -Math.Log(P) / delta_total;

                ZC.Add(P);
                yields.Add(y);
                ZCDict.Add(fi, P);

                j++;
            }

            return yields;
        }
    }
}
