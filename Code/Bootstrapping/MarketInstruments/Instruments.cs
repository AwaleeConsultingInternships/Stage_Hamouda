namespace Bootstrapping.Instruments
{
    public class Instruments
    {
        private Instrument[] _swaps;
        public Instrument[] Swaps
        {
            get { return _swaps; }
            set { _swaps = value; }
        }

        private Instrument[] _futures;
        public Instrument[] Futures
        {
            get { return _futures; }
            set { _futures = value; }
        }

        public Instruments(Instrument[] swaps, Instrument[] futures)
        {
            _swaps = swaps;
            _futures = futures;
        }

        public override string ToString()
        {
            return "Instruments: " + _swaps.Length + " Swaps" + " and " + _futures.Length + " Futures.";
        }
    }
}
