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
            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(3, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingDirectSolving;
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.InterpolatedData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);


            // Load the data
            var jsonContent = Utilities.Directories.GetJsonContent();
            var deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            // Store the Swaps maturities and rates in a dictionnary
            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.Swaps);

            var algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);


            Console.ReadKey();
        }
    }
}
