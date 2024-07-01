using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
using Newtonsoft.Json;

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


        }
    }
}
