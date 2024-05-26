using Bootstrapping;
using Interpolation;
using MathematicalTools;
using Newtonsoft.Json;
using Utilities;

namespace Tests.Interpolation
{
    internal class Plotting
    {
        [Test]
        public void Test1()
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            //Console.WriteLine(jsonContent);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            Dictionary<double, double> swapRates = new Dictionary<double, double>();
            foreach (var swap in deserializedObject.swaps)
            {
                var dd = int.Parse(swap.maturity.Substring(0, swap.maturity.Length - 1));
                swapRates.Add(dd, swap.rate);
            }

            var discount = BootstrappingClass.Curve(swapRates);

            var nbYears = 40;
            var shift = 21;
            var list = new List<Point>();
            for (int i = 0; i < 365 * nbYears; i+= shift)
            {
                list.Add(new Point(i / 365.0, discount.Evaluate(i / 365.0)));
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "chart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
    }
}
