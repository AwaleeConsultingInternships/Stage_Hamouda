namespace Bootstrapping.Instruments
{
    public class Future
    {
        private string _date;
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private double _rate;
        public double Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        public Future(string date, double rate)
        {
            _date = date;
            _rate = rate;
        }
    }
}
