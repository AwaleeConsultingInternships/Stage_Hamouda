﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MathematicalFunctions;
using Bootstrapping;

namespace Interpolation
{
    public class Program
    {
        public static string GetMarketDataPath()
        {
            // Create relative path
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Directory.GetParent(exeDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName + "\\MarketData\\";
        }

        static void Main(string[] args)
        {
            var projectDirectory = GetMarketDataPath();
            string filePath = Path.Combine(projectDirectory, "swaps.json");

            Console.WriteLine(filePath);

            // Initialisation
            Dictionary<double, double> swapRates = new Dictionary<double, double>();

            if (File.Exists(filePath))
            {
                // Load the data
                string jsonContent = File.ReadAllText(filePath);
                Console.WriteLine(jsonContent);
                RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                // Store and display the swaps maturities and rates
                Console.WriteLine("Swaps' maturities and rates: ");
                foreach (var swap in deserializedObject.swaps)
                {
                    var dd = int.Parse(swap.maturity.Substring(0, swap.maturity.Length - 1));
                    swapRates.Add(dd, swap.rate);

                }

                var discount = BootstrappingClass.Curve(swapRates);
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
