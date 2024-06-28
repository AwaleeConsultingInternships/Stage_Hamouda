using Bootstrapping.Instruments;

namespace Bootstrapping.MarketInstruments
{
    public class Future : Instrument
    {
        public Future(InstrumentType instrumentType,
            string maturity, double rate)
            : base(instrumentType, maturity, rate)
        {
        }
    }
}
