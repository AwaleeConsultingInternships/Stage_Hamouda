using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MathematicalFunctions;
using Bootstrapping;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Interpolation
{
    public class Program
    {
        public static Period GetMaturity(string maturity)
        {
            var mat = int.Parse(maturity.Substring(0, maturity.Length - 1));
            var unit = maturity[maturity.Length - 1];
            switch (unit)
            {
                case 'Y':
                    return new Period(mat, Unit.Years);
                case 'M':
                    return new Period(mat, Unit.Months);
                case 'D':
                    return new Period(mat, Unit.Days);
                case 'W':
                    return new Period(mat, Unit.Weeks);
                default:
                    throw new ArgumentException("Invalid period format. Expected format is '1Y', '2M', or '30D'.");
            }
        }

        public 

        static void Main(string[] args)
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            // Initialisation
            Dictionary<Period, double> swapRates = new Dictionary<Period, double>();

            if (File.Exists(filePath))
            {
                // Load the data
                string jsonContent = File.ReadAllText(filePath);
                //Console.WriteLine(jsonContent);
                RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                // Store the swaps maturities and rates in a dictionnary
                
                foreach (var swap in deserializedObject.swaps)
                {
                    string maturity = swap.maturity;
                    Period p = GetMaturity(maturity);
                    swapRates.Add(p, swap.rate);
                }
                var freq = new PeriodConventioned(new Period(3, Unit.Months), new DayCounter(DayConvention.ACT365));
                var discount = BootstrappingClass.Curve(swapRates, freq); //Exp: 3M
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
