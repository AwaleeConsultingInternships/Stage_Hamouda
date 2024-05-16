using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MathematicalFunctions;

namespace Interpolation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create relative path
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(exeDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(projectDirectory, "swaps.json");

            Console.WriteLine(filePath);

            // Initialisation
            List<string> maturities = new List<string>();
            List<double> rates = new List<double>();
            List<double> ZC = new List<double>();
            List<double> yields = new List<double>();
            double P;
            double y;

            if (File.Exists(filePath))
            {
                // Load the data
                string jsonContent = File.ReadAllText(filePath);
                Console.WriteLine(jsonContent);
                RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                // Store and display the swaps maturities
                Console.WriteLine("Swaps' maturities: ");
                foreach (var swap in deserializedObject.swaps)
                {
                    maturities.Add(swap.maturity);
                    Console.WriteLine(maturities[maturities.Count - 1]);
                }


                // Store and display the swap rates
                Console.WriteLine("\nCorresponding rates ");
                foreach (var swap in deserializedObject.swaps)
                {
                    rates.Add(swap.rate);
                    Console.WriteLine(rates[rates.Count - 1]);
                }

                // Compute and display the ZC prices and yield curve values for the given maturities only
                Console.WriteLine("\nZero-Coupon prices: ");
                int delta = int.Parse(maturities[0].Substring(0, maturities[0].Length - 1));
                int delta_total = delta;
                double Q = 0;
                P = 1 / (1 + rates[0] * delta);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);
                Console.WriteLine(P);

                for (int i = 1; i < maturities.Count; i++)
                {
                    Q += P * delta;
                    delta = int.Parse(maturities[i].Substring(0, maturities[i].Length - 1)) - int.Parse(maturities[i - 1].Substring(0, maturities[i - 1].Length - 1));
                    delta_total = int.Parse(maturities[i].Substring(0, maturities[i].Length - 1));

                    P = (1 - rates[i] * Q) / (1 + rates[i] * delta);
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
                Piecewise_Linear YieldF = new Piecewise_Linear();
                for (int i=1; i < maturities.Count; i++)
                {
                    double x1 = int.Parse(maturities[i-1].Substring(0, maturities[i-1].Length - 1));
                    double y1 = yields[i - 1];
                    double x2 = int.Parse(maturities[i].Substring(0, maturities[i].Length - 1));
                    double y2 = yields[i];
                    YieldF.AddInterval(x1, y1, x2, y2);
                }

                // Compute y(0, 4Y)
                double yield4Y = YieldF.Evaluate(4);
                Console.WriteLine($"\nInterpolated yield value at 4Y: {yield4Y}");


                // Define the function: t -> P(0, t) = Exp(-y(0,t)*t)
                Discount ZCFunc = new Discount(YieldF);

                // Compute P(0, 4Y)
                double ZC4Y = ZCFunc.Evaluate(4);
                Console.WriteLine($"\nInterpolated ZC price at 4Y: {ZC4Y}");
                

                
            }
            else
            {
                Console.WriteLine("The JSON file does not exist.");
            }

            Console.ReadKey();
        }

    }

    public class RootObject
    {
        public Swap[] swaps { get; set; }
    }

    public class Swap
    {
        public string maturity { get; set; }
        public double rate { get; set; }
    }
}
