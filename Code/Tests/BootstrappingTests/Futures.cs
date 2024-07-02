using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
using Bootstrapping.YieldComputer;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Tests.BootstrappingTests
{
    public class Futures
    {
        [Test]
        public void Test()
        {
            var marketDataDirectory = Utilities.Directories.GetMarketDataDirectory();
            string filePath = Path.Combine(marketDataDirectory, "futures.json");

            string jsonContent = File.ReadAllText(filePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var futureRates = InstrumentParser.GetFutureRates(deserializedObject.MarketInstruments);

            var pricingDate = new Date(19, 06, 2024);
            var period = new Period(3, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters);

            Algorithm algorithm = new Algorithm(bootstrappingParameters);

            var discount = algorithm.Curve(futureRates);

            foreach (var future in futureRates)
            {
                var date = future.Key;
                var price = FuturePricer.Pricer(date, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(futureRates[date]).Within(1e-6));
            }
        }
    }
}
