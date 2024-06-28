namespace Bootstrapping.Instruments
{
    public class Instruments
    {
        private Instrument[] _marketInstruments;
        public Instrument[] MarketInstruments
        {
            get { return _marketInstruments; }
            set { _marketInstruments = value; }
        }

        public Instruments(Instrument[] swaps)
        {
            _marketInstruments = swaps;
        }
    }
}
