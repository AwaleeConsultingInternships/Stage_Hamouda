using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Tests.BootstrappingTests
{
    public class Repricing
    {
        public static Array GetNumberOfMonths()
        {
            return new int[] { 3, 6, 12 };
        }

        public static Array GetInterpolationChoices()
        {
            return Enum.GetValues(typeof(InterpolationChoice));
        }

        public static Array GetVariableChoices()
        {
            return Enum.GetValues(typeof(VariableChoice));
        }

        [Test]
        public void InterpolatedData([ValueSource(nameof(GetNumberOfMonths))] int numberOfMonth,
            [ValueSource(nameof(GetInterpolationChoices))] InterpolationChoice interpolationChoice,
            [ValueSource(nameof(GetVariableChoices))] VariableChoice variableChoice)
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "Swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(numberOfMonth, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var dataChoice = DataChoice.InterpolatedData;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, dataChoice, variableChoice);

            Algorithm algorithm = new Algorithm(bootstrappingParameters);

            var discount = algorithm.Curve(swapRates);

            foreach (var swap in swapRates)
            {
                var maturity = swap.Key;
                var price = SwapPricer.Pricer(maturity, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-6));
            }
        }

        [Test]
        public void RawData([ValueSource(nameof(GetNumberOfMonths))] int numberOfMonth, 
            [ValueSource(nameof(GetVariableChoices))] VariableChoice variableChoice)
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "Swaps.json");

            string jsonContent = File.ReadAllText(filePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(numberOfMonth, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var dataChoice = DataChoice.RawData;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, dataChoice, variableChoice);

            Algorithm algorithm = new Algorithm(bootstrappingParameters);

            var discount = algorithm.Curve(swapRates);

            foreach (var swap in swapRates)
            {
                var maturity = swap.Key;
                var price = SwapPricer.Pricer(maturity, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-8));
            }
        }
    }
}
