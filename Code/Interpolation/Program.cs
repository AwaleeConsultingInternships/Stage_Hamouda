using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Interpolation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //c'est mieux de faire des chemin relatifs
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "swaps.json");
            Console.WriteLine(filePath);

            List<string> maturities = new List<string>();
            List<double> rates = new List<double>();
            List<double> ZC = new List<double>();
            List<double> yields = new List<double>();
            double P;
            double y;

            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                Console.WriteLine(jsonContent);
                RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                Console.WriteLine("Swaps' maturities: ");
                foreach (var swap in deserializedObject.swaps)
                {
                    maturities.Add(swap.maturity);
                    Console.WriteLine(maturities[maturities.Count - 1]);
                }


                Console.WriteLine("\nCorresponding rates ");
                foreach (var swap in deserializedObject.swaps)
                {
                    rates.Add(swap.rate);
                    Console.WriteLine(rates[rates.Count - 1]);
                }

                Console.WriteLine("\nZero-Coupon prices: ");
                int delta = int.Parse(maturities[0].Substring(0, maturities[0].Length - 1));
                int delta_total = delta;
                double Q = 0;
                P = 1 / (1 + rates[0] * delta);
                y = -Math.Log(P) / delta_total;
                ZC.Add(P);
                yields.Add(y);
                Console.WriteLine(P);

                // on a supposé que les swaps payent chaque année => Hypthèse très importante pour le moment

                // il y a des maturités intermédiaires manquantes
                // comment gérer P(0, 4Y) et y(0, 4Y) dans l'algorithme ?
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

                // définir la fonction t -> y(0, t)


                // finalité : t -> P(0, t)
                Console.WriteLine("\nYield values: ");
                foreach (var yield in yields)
                {
                    Console.WriteLine(yield);
                }
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
