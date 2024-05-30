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
            if (maturity.EndsWith("Y"))
            {
                return new Period(int.Parse(maturity.Substring(0, maturity.Length - 1)), Unit.Years);
            }
            else if (maturity.EndsWith("M"))
            {
                return new Period(int.Parse(maturity.Substring(0, maturity.Length - 1)), Unit.Months);
            }
            else if (maturity.EndsWith("W"))
            {
                return new Period(int.Parse(maturity.Substring(0, maturity.Length - 1)), Unit.Weeks);
            }
            else if (maturity.EndsWith("D"))
            {
                return new Period(int.Parse(maturity.Substring(0, maturity.Length - 1)), Unit.Days);
            }
            else
            {
                throw new ArgumentException("Invalid period format. Expected format is '1Y', '2M', or '30D'.");
            }
        }
        static void Main(string[] args)
        {
            var pricingDate = new Date(01, 05, 2024);
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            // Initialisation
            Dictionary<double, double> swapRates = new Dictionary<double, double>();

            if (File.Exists(filePath))
            {
                // Load the data
                string jsonContent = File.ReadAllText(filePath);
                Console.WriteLine(jsonContent);
                RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                // Store the swaps maturities and rates in a dictionnary
                foreach (var swap in deserializedObject.swaps)
                {
                    string maturity = swap.maturity;
                    Period p = GetMaturity(maturity);
                    Date maturityDate = pricingDate.Advance(p);
                    DayCounter maturityDouble = new DayCounter(DayConvention.ACT365);
                    double dd = maturityDouble.YearFraction(pricingDate, maturityDate);
                    Console.WriteLine(dd);
                    swapRates.Add(dd, swap.rate);
                }
                var discount = BootstrappingClass.Curve(swapRates, 0.25); //Exp: 3M
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
