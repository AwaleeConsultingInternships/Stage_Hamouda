﻿using Bootstrapping;
using Interpolation;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Tests.Interpolation
{
    public class Repricing
    {
        [TestCase(3, false)]
        [TestCase(3, true)]
        [TestCase(6, false)]
        [TestCase(6, true)]
        [TestCase(12, false)]
        [TestCase(12, true)]
        public void Eval(int testP, bool root)
        {
            //var pricingDate = new Date(01, 05, 2024);
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            var swapRates = Bootstrapping.Utilities.GetSwapRates(deserializedObject.swaps);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(testP, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var bootstrappingParameters = new BootstrappingParameters(pricingDate, period, dayCouner, root);

            BootstrappingAlgorithm bootstrapping = root
                ? new BootstrappingUsingNewton(bootstrappingParameters)
                : new BootstrappingUsingDirectSolving(bootstrappingParameters);
            var discount = bootstrapping.Curve(swapRates);

            foreach(var swap in swapRates)
            {
                var maturity = swap.Key;
                var price = SwapPricer.Pricer(maturity, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-6));
            }
        }
    }
}
