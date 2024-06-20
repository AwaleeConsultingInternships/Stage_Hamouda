using Bootstrapping;
using Interpolation;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Tests.Interpolation
{
    public class Repricing
    {
        [TestCase(3, InterpolationChoice.UsingDirectSolving, DataChoice.InterpolatedData)]
        [TestCase(3, InterpolationChoice.UsingNewtonSolver, DataChoice.InterpolatedData)]
        [TestCase(3, InterpolationChoice.UsingRawData, DataChoice.RawData)]
        [TestCase(6, InterpolationChoice.UsingDirectSolving, DataChoice.InterpolatedData)]
        [TestCase(6, InterpolationChoice.UsingNewtonSolver, DataChoice.InterpolatedData)]
        [TestCase(6, InterpolationChoice.UsingRawData, DataChoice.RawData)]
        [TestCase(12, InterpolationChoice.UsingDirectSolving, DataChoice.InterpolatedData)]
        [TestCase(12, InterpolationChoice.UsingNewtonSolver, DataChoice.InterpolatedData)]
        [TestCase(12, InterpolationChoice.UsingRawData, DataChoice.RawData)]
        public void Eval(int testP, InterpolationChoice interpolationChoice, DataChoice dataChoice)
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
            var newtonSolverParameters = new NewtonSolverParameters();
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, dataChoice);

            Algorithm algorithm = new Algorithm(bootstrappingParameters);

            var discount = algorithm.Curve(swapRates);

            foreach (var swap in swapRates)
            {
                var maturity = swap.Key;
                var price = SwapPricer.Pricer(maturity, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-6));
            }
        }
    }
}
