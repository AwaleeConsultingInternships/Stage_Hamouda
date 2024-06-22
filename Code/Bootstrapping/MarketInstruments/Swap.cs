namespace Bootstrapping.Instruments
{
    public class Swap
    {
        private string _maturity;
        public string Maturity
        {
            get { return _maturity; }
            set { _maturity = value; }
        }

        private double _rate;
        public double Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        public Swap(string maturity, double rate)
        {
            _maturity = maturity;
            _rate = rate;
        }
    }
}
