using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;

namespace Bootstrapping
{
    public static class BootstrappingClass
    {
        public static Discount Curve(Dictionary<double, double> swapRates)
        {
            List<double> ZC = new List<double>();
            List<double> yields = new List<double>();

            double P;
            double y;

            // Interpoalte the missing values for the swap rates
            PiecewiseLinear SwapInt = new PiecewiseLinear();

            var maturities = swapRates.Keys.ToArray();
            for (int i = 1; i < swapRates.Keys.Count; i++)
            {
                double x1 = maturities[i - 1];
                double y1 = swapRates[x1];
                double x2 = maturities[i];
                double y2 = swapRates[x2];
                SwapInt.AddInterval(x1, y1, x2, y2);
            }

            double Final_maturity = swapRates.Keys.Last();
            for (int i = 1; i < Final_maturity; i++)
            {
                if (!maturities.Contains(i))
                {
                    double sw = SwapInt.Evaluate(i);
                    swapRates.Add(i, sw);
                }
            }
            swapRates = swapRates.OrderBy(pair => pair.Key).ToDictionary(x => x.Key, x => x.Value);

            maturities = swapRates.Keys.ToArray();


            // Compute and display the ZC prices and yield curve values for the given maturities only
            Console.WriteLine("\nZero-Coupon prices: ");
            double delta = maturities.First();
            double delta_total = delta;
            double Q = 0;
            P = 1 / (1 + swapRates[1] * delta);
            y = -Math.Log(P) / delta_total;
            ZC.Add(P);
            yields.Add(y);
            Console.WriteLine(P);

            for (int i = 1; i < maturities.Length; i++)
            {
                Q += P * delta;
                delta = maturities[i] - maturities[i - 1];
                delta_total = maturities[i];

                P = (1 - swapRates[maturities[i]] * Q) / (1 + swapRates[maturities[i]] * delta);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);
                Console.WriteLine(P);
            }

            Console.WriteLine("\nYield values: ");
            foreach (var yield in yields)
            {
                Console.WriteLine(yield);
            }

            // Define the function: t -> y(0, t)
            PiecewiseLinear YieldF = new PiecewiseLinear();
            for (int i = 1; i < maturities.Length; i++)
            {
                double x1 = maturities[i - 1];
                double y1 = yields[i - 1];
                double x2 = maturities[i];
                double y2 = yields[i];
                YieldF.AddInterval(x1, y1, x2, y2);
            }

            // Compute y(0, 3.5Y)
            double yieldInt = YieldF.Evaluate(3.5);
            Console.WriteLine($"\nInterpolated yield value at 3.5Y: {yieldInt}");


            return new Discount(YieldF);
        }
    }
}
