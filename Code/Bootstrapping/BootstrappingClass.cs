using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;
using QuantitativeLibrary.Time;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;

namespace Bootstrapping
{
    public static class BootstrappingClass
    {
        public static Discount Curve(Dictionary<Period, double> swapRates, BootstrappingParameters bootstrappedParameters)
        {
            List<double> ZC = new List<double>();
            List<double> yields = new List<double>();

            Dictionary<Period, double> ZCDict = new Dictionary<Period, double>();

            double P;
            double y;

            var pricingDate = bootstrappedParameters.PricingDate;
            var counter = bootstrappedParameters.DayCounter;
            var periodicity = bootstrappedParameters.Periodicity;
            var solveRoot = bootstrappedParameters.SolveRoot;
            var target = bootstrappedParameters.Target;
            var firstGuess = bootstrappedParameters.FirstGuess;
            var tolerance = bootstrappedParameters.Tolerance;

            // Interpoalte the missing values for the swap rates
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
            SwapInt.AddInterval(lastX, swapRates[lastPeriod], Double.PositiveInfinity, swapRates[lastPeriod]);

            var lastMaturity = Utilities.ConvertYearsToMonths(swapRates.Keys.Last());

            var interpolatedSwapRates = Utilities.InterpolateSwapRate(SwapInt, swapRates, bootstrappedParameters);

            // Compute and store the ZC prices and yield curve values for the given maturities
            if (solveRoot)
            {
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
                    swapFunc = SwapFunc.SwapValue(ZCDict, interpolatedSwapRates[fi], bootstrappedParameters);
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

            }
            else
            {
                var f = Utilities.Duration(periodicity, pricingDate, counter);
                double delta_total = f;
                double Q = 0;
                var firstSwap = interpolatedSwapRates[periodicity];
                P = 1 / (1 + firstSwap * f);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);

                var datePrevious = pricingDate;
                var date = pricingDate.Advance(periodicity);

                var j = 2;
                while (j * periodicity.NbUnit <= lastMaturity.NbUnit)
                {
                    datePrevious = date;
                    date = date.Advance(periodicity);
                    Q += P * f;
                    f = counter.YearFraction(datePrevious, date);
                    Period fi = new Period(j * periodicity.NbUnit, periodicity.Unit);
                    P = (1 - interpolatedSwapRates[fi] * Q) / (1 + interpolatedSwapRates[fi] * f);
                    delta_total += f;
                    y = -Math.Log(P) / delta_total;
                    ZC.Add(P);
                    yields.Add(y);
                
                    j++;
                }
            }

            // Define the function: t -> y(0, t)
            PiecewiseLinear YieldF = new PiecewiseLinear();

            var datePreviousN = pricingDate;
            var dateN = pricingDate.Advance(periodicity);

            var fN = counter.YearFraction(datePreviousN, dateN);

            YieldF.AddInterval(0, 0, fN, yields[0]);

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

            YieldF.AddInterval(xFinal, yields.Last(), Double.PositiveInfinity, yields.Last());


            return new Discount(YieldF);
        }
    }
}
