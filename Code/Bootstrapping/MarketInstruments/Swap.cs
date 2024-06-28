using Bootstrapping.Instruments;

namespace Bootstrapping.MarketInstruments
{
    public class Swap : Instrument
    {
        public Swap(InstrumentType instrumentType,
            string maturity, double rate)
            : base(instrumentType, maturity, rate)
        {
        }
    }
}
