using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public static class BootstrappingClass
    {
        public static double Duration(Period p, Date pricingDate, DayCounter counter)
        {
            Date maturityDate = pricingDate.Advance(p);
            return counter.YearFraction(pricingDate, maturityDate);
        }

        public static Discount Curve(Dictionary<Period, double> swapRates, PeriodConventioned freq)
        {
            List<double> ZC = new List<double>();
            List<double> yields = new List<double>();

            double P;
            double y;

            var pricingDate = new Date(01, 05, 2024);
            var counter = freq.dayCounter;

            // Interpoalte the missing values for the swap rates
            PiecewiseLinear SwapInt = new PiecewiseLinear();

            var maturities = swapRates.Keys.ToArray();
            for (int i = 1; i < swapRates.Keys.Count; i++)
            {
                Period p1 = maturities[i - 1];
                double x1 = Duration(p1, pricingDate, counter);
                double y1 = swapRates[p1];
                Period p2 = maturities[i];
                double x2 = Duration(p2, pricingDate, counter);
                double y2 = swapRates[p2];
                SwapInt.AddInterval(x1, y1, x2, y2);
            }

            Period Final_maturity = swapRates.Keys.Last();
            int j = 1;
            int f = freq.period.NbUnit;
            while (j * f < Final_maturity.NbUnit)
            {
                var p = new Period (j*f, freq.period.Unit);
                if (!maturities.Contains(p))
                {
                    double x = Duration(p, pricingDate, counter);
                    double sw = SwapInt.Evaluate(x);
                    swapRates.Add(p, sw);
                }
                j++;
            }

            swapRates = swapRates.OrderBy(pair => pair.Key).ToDictionary(x => x.Key, x => x.Value);

            maturities = swapRates.Keys.ToArray();


            // Compute and store the ZC prices and yield curve values for the given maturities
            double delta_total = f;
            double Q = 0;
            P = 1 / (1 + swapRates[freq.period] * f);
            y = -Math.Log(P) / delta_total;
            ZC.Add(P);
            yields.Add(y);

            j = 2;
            while (j * f <= Final_maturity.NbUnit)
            {
                Q += P * f;
                delta_total = j * f;
                Period fi = new Period(j * f, freq.period.Unit);
                P = (1 - swapRates[fi] * Q) / (1 + swapRates[fi] * f);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);
                j++;
            }

            // Define the function: t -> y(0, t)
            PiecewiseLinear YieldF = new PiecewiseLinear();
            YieldF.AddInterval(0, 0, f, yields[0]);

            for (int i = 1; i < yields.Count; i++)
            {
                double x1 = i * f;
                double y1 = yields[i - 1];
                double x2 = (i + 1) * f;
                double y2 = yields[i];
                YieldF.AddInterval(x1, y1, x2, y2);
            }

            YieldF.AddInterval(yields.Count * f, yields.Last(), Double.PositiveInfinity, yields.Last());

            // Compute y(0, 3.5Y)
            //double yieldInt = YieldF.Evaluate(3.5);
            //Console.WriteLine($"\nInterpolated yield value at 3.5Y: {yieldInt}");


            return new Discount(YieldF);
        }
    }
}
