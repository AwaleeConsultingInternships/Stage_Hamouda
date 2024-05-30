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
            var pricingDate = new Date(01, 05, 2024);
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            Dictionary<double, double> swapRates = new Dictionary<double, double>();
            foreach (var swap in deserializedObject.swaps)
            {
                string maturity = swap.maturity;
                Period p = Program.GetMaturity(maturity);
                Date maturityDate = pricingDate.Advance(p);
                DayCounter maturityDouble = new DayCounter(DayConvention.ACT365);
                double dd = maturityDouble.YearFraction(pricingDate, maturityDate);
                Console.WriteLine(dd);
                swapRates.Add(dd, swap.rate);
            }

            var discount = BootstrappingClass.Curve(swapRates, 0.25);

            for (int maturity = 1; maturity <= (int)(30 / 0.25); maturity++)
            {
                var price = SwapPricer.Pricer(maturity * 0.25, discount, 0.25);
                Console.WriteLine(maturity * 0.25);
                Console.WriteLine(price);
                Assert.That(price, Is.EqualTo(swapRates[maturity * 0.25]).Within(1e-10));
            }
        }
    }
}
