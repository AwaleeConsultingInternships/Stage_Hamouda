namespace Bootstrapping.Instruments
{
    public enum InstrumentType
    {
        Swap,
        Future
    }

    public class Instrument
    {
        private InstrumentType _instrumentType;
        public InstrumentType InstrumentType
        {
            get { return _instrumentType; }
            set { _instrumentType = value; }
        }

        private string _maturity;
        public string Maturity
        {
            get { return _maturity; }
            set { _maturity = value; }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Instrument(InstrumentType instrumentType,
            string maturity, double rate)
        {
            _maturity = maturity;
            _value = rate;
        }
    }
}
