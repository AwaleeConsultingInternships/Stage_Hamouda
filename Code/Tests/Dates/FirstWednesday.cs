using Bootstrapping.MarketInstruments;
using QuantitativeLibrary.Time;

namespace Tests.Dates
{
    public class FirstWednesday
    {
        [Test]
        public void FirstWednesdayTest()
        {
            Date x = InstrumentParser.GetThirdWednesday("JUN 2025");
            string y = "18/6/2025";
            Assert.That(x.ToString, Is.EqualTo(y));
        }
    }
}
