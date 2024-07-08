namespace Bootstrapping.Instruments
{
    public class Instrument
    {
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

        public Instrument(string maturity, double rate)
        {
            _maturity = maturity;
            _value = rate;
        }
    }
}
