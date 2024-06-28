using Newtonsoft.Json;
using Bootstrapping;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;
using Bootstrapping.Instruments;
using Bootstrapping.CurveParameters;
using Bootstrapping.MarketInstruments;

namespace Interpolation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "Swaps.json");

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(3, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingDirectSolving;
            var dataChoice = DataChoice.InterpolatedData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, dataChoice, variableChoice);

            if (File.Exists(filePath))
            {
                // Load the data
                string jsonContent = File.ReadAllText(filePath);
                //Console.WriteLine(jsonContent);
                Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

                // Store the Swaps maturities and rates in a dictionnary
                var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

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
}
