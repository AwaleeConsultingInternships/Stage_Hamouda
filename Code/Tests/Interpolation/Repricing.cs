using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalFunctions;
using Bootstrapping;
using Interpolation;
using Newtonsoft.Json;

namespace Tests.Interpolation
{
    public class Repricing
    {
        [Test]
        public void Eval()
        {
            var projectDirectory = Program.GetMarketDataPath();
            string filePath = Path.Combine(projectDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            //Console.WriteLine(jsonContent);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            Dictionary<double, double> swapRates = new Dictionary<double, double>();
            foreach (var swap in deserializedObject.swaps)
            {
                var dd = int.Parse(swap.maturity.Substring(0, swap.maturity.Length - 1));
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
