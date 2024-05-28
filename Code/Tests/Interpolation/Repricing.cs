using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;
using Bootstrapping;
using Interpolation;
using Newtonsoft.Json;
using Time;

namespace Tests.Interpolation
{
    public class Repricing
    {
        [Test]
        public void Eval()
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            Dictionary<double, double> swapRates = new Dictionary<double, double>();
            foreach (var swap in deserializedObject.swaps)
            {
                var dd = Time.Time.GetMaturity(swap.maturity);
                swapRates.Add(dd, swap.rate);
            }

            var discount = BootstrappingClass.Curve(swapRates);

            for (int maturity = 1; maturity <= 30; maturity++)
            {
                var price = SwapPricer.Pricer(maturity, discount);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-10));
            }
        }
    }
}
