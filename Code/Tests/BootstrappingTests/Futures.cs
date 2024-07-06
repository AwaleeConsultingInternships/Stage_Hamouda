using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
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
            var jsonContent = Utilities.Directories.GetJsonContent();
            var marketInstruments = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var futureRates = InstrumentParser.GetFutureRates(marketInstruments.Futures);

            var pricingDate = new Date(19, 06, 2024);
            var period = new Period(3, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingFuturesMain;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice);

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
