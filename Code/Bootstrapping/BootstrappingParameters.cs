using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class BootstrappingParameters
    {
        private Date _pricingDate;
        private Period _periodicity;
        private DayCounter _dayCounter;
        private bool _solveRoot;
        private double _target;
        private double _firstGuess;
        private double _tolerance;

        public Date PricingDate
        {
            get { return _pricingDate; }
            set { _pricingDate = value; }
        }

        public Period Periodicity
        {
            get { return _periodicity; }
            set { _periodicity = value; }
        }

        public DayCounter DayCounter
        {
            get { return _dayCounter; }
            set { _dayCounter = value; }
        }

        public bool SolveRoot
        {
            get { return _solveRoot; }
            set { _solveRoot = value; }
        }

        public double Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public double FirstGuess
        {
            get { return _firstGuess; }
            set { _firstGuess = value; }
        }

        public double Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }

        public BootstrappingParameters(Date pricingDate, Period periodicity, DayCounter dayCounter, bool solveRoot = false, double target = 0, double firstGuess = 1, double tolerance = 1e-10)
        {
            _pricingDate = pricingDate;
            _periodicity = periodicity;
            _dayCounter = dayCounter;
            _solveRoot = solveRoot;
            _target = target;
            _firstGuess = firstGuess;
            _tolerance = tolerance;
        }

        public override string ToString()
        {
            return string.Format("Pricing date = {0}, Periodicity = {1}, Day counter = {2}, Solve root = {3}, Target = {4}, First guess = {5}, Tolerance = {6}",
                _pricingDate, _periodicity, _dayCounter, _solveRoot, _target, _firstGuess, _tolerance);
        }
    }
}
