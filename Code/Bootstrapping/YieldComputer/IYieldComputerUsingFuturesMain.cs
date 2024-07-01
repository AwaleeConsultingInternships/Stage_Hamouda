﻿using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Maths.Solver.RootFinder;
using QuantitativeLibrary.Time;

namespace Bootstrapping.YieldComputer
{

    public class YieldComputerUsingFuturesMain : IYieldComputerUsingFutures
    {
        public Parameters _parameters;
        public Parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public YieldComputerUsingFuturesMain(Parameters parameters)
        {
            _parameters = parameters;
        }

        public List<double> Compute(Dictionary<Date, double> futureRates)
        {
            var pricingDate = _parameters.PricingDate;
            var counter = _parameters.DayCounter;
            var periodicity = _parameters.Periodicity;
            var newtonSolverParameters = _parameters.NewtonSolverParameters;
            var target = newtonSolverParameters.Target;
            var firstGuess = newtonSolverParameters.FirstGuess;
            var tolerance = newtonSolverParameters.Tolerance;

            List<double> yields = new List<double>();
            List<double> ZC = new List<double>();

            double P = 1;
            double y;

            var previousDate = pricingDate;

            foreach (var futureRate in futureRates)
            {
                var date = futureRate.Key;
                var rate = futureRate.Value;

                var futureFunc = FutureFunc.FutureValue(previousDate, P, date, rate, _parameters);
                var solver = NewtonSolver.CreateWithAbsolutePrecision(target, futureFunc, futureFunc.FirstDerivative, firstGuess, tolerance);
                var result = solver.Solve();

                var deltaTotal = counter.YearFraction(pricingDate, date);

                P = IYieldComputer.GetDiscount(result.Solution, deltaTotal, Parameters);
                y = IYieldComputer.GetYield(result.Solution, deltaTotal, Parameters);

                ZC.Add(P);
                yields.Add(y);

                previousDate = date;
            }

            return yields;
        }
    }
}
