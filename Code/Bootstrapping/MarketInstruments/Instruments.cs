namespace Bootstrapping.Instruments
{
    public class Instruments
    {
        private Swap[] _swaps;
        public Swap[] Swaps
        {
            get { return _swaps; }
            set { _swaps = value; }
        }

        public Instruments(Swap[] swaps)
        {
            _swaps = swaps;
        }
    }
}
