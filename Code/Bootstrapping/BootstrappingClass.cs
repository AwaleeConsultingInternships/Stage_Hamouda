using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;

namespace Bootstrapping
{
    public static class BootstrappingClass
    {
        public static Discount Curve(Dictionary<double, double> swapRates, double freq)
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
            int j = 1;
            while (j * freq < Final_maturity)
            {
                if (!maturities.Contains(j * freq))
                {
                    double sw = SwapInt.Evaluate(j * freq);
                    swapRates.Add(j * freq, sw);
                }
                j++;
            }

            swapRates = swapRates.OrderBy(pair => pair.Key).ToDictionary(x => x.Key, x => x.Value);

            maturities = swapRates.Keys.ToArray();


            // Compute and store the ZC prices and yield curve values for the given maturities
            double delta_total = freq;
            double Q = 0;
            P = 1 / (1 + swapRates[freq] * freq);
            y = -Math.Log(P) / delta_total;
            ZC.Add(P);
            Console.WriteLine(swapRates[freq]);
            Console.WriteLine(P);
            yields.Add(y);

            j = 2;
            while (j * freq <= Final_maturity)
            {
                Q += P * freq;
                delta_total = j * freq;
                P = (1 - swapRates[j * freq] * Q) / (1 + swapRates[j * freq] * freq);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);
                j++;
            }

            /*for (int i = 1; i < maturities.Length; i++)
            {
                Q += P * delta;
                delta = maturities[i] - maturities[i - 1];
                delta_total = maturities[i];

                P = (1 - swapRates[maturities[i]] * Q) / (1 + swapRates[maturities[i]] * delta);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);
            }*/

            // Define the function: t -> y(0, t)
            PiecewiseLinear YieldF = new PiecewiseLinear();
            YieldF.AddInterval(0, 0, freq, yields[0]);

            for (int i = 1; i < yields.Count; i++)
            {
                double x1 = i * freq;
                double y1 = yields[i - 1];
                double x2 = (i + 1) * freq;
                double y2 = yields[i];
                YieldF.AddInterval(x1, y1, x2, y2);
            }

            YieldF.AddInterval(yields.Count * freq, yields.Last(), Double.PositiveInfinity, yields.Last());

            // Compute y(0, 3.5Y)
            //double yieldInt = YieldF.Evaluate(3.5);
            //Console.WriteLine($"\nInterpolated yield value at 3.5Y: {yieldInt}");


            return new Discount(YieldF);
        }
    }
}
