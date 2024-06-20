using Newtonsoft.Json;
using Bootstrapping;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Interpolation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "swaps.json");

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(3, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingDirectSolving;
            var dataChoice = DataChoice.InterpolatedData;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, dataChoice);

            if (File.Exists(filePath))
            {
                // Load the data
                string jsonContent = File.ReadAllText(filePath);
                //Console.WriteLine(jsonContent);
                RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                // Store the swaps maturities and rates in a dictionnary
                var swapRates = Bootstrapping.Utilities.GetSwapRates(deserializedObject.swaps);

                var algorithm = new Algorithm(bootstrappingParameters);
                var discount = algorithm.Curve(swapRates);
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
}
