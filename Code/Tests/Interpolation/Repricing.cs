using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;
using Bootstrapping;
using Interpolation;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Tests.Interpolation
{
    public class Repricing
    {
        [Test]
        public void Eval()
        {
            //var pricingDate = new Date(01, 05, 2024);
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            Dictionary<Period, double> swapRates = new Dictionary<Period, double>();
            
            foreach (var swap in deserializedObject.swaps)
            {
                string maturity = swap.maturity;
                Period p = Program.GetMaturity(maturity);
                swapRates.Add(p, swap.rate);
            }

            var freq = new PeriodConventioned(new Period(6, Unit.Months), new DayCounter(DayConvention.ACT365));

            var discount = BootstrappingClass.Curve(swapRates, freq);
            var pricingDate = new Date(01, 05, 2024);
            var f = BootstrappingClass.Duration(freq.period, pricingDate, freq.dayCounter);

            for (int i = 1; i <= (int)(30 / f); i++)
            {
                var maturity = new Period(i * freq.period.NbUnit, freq.period.Unit);
                var price = SwapPricer.Pricer(maturity, discount, freq);
                //Console.WriteLine(i * f);
                //Console.WriteLine(price);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-10));
            }
        }
    }
}
