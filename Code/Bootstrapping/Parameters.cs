using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class Parameters
    {
        private Date _pricingDate;
        public Date PricingDate
        {
            get { return _pricingDate; }
            set { _pricingDate = value; }
        }

        private Period _periodicity;
        public Period Periodicity
        {
            get { return _periodicity; }
            set { _periodicity = value; }
        }

        private DayCounter _dayCounter;
        public DayCounter DayCounter
        {
            get { return _dayCounter; }
            set { _dayCounter = value; }
        }

        private NewtonSolverParameters _newtonSolverParameters;
        public NewtonSolverParameters NewtonSolverParameters
        {
            get { return _newtonSolverParameters; }
            set { _newtonSolverParameters = value; }
        }

        private InterpolationChoice _interpolationChoice;
        public InterpolationChoice InterpolationChoice
        {
            get { return _interpolationChoice; }
            set { _interpolationChoice = value; }
        }

        public Parameters(Date pricingDate, Period periodicity, DayCounter dayCounter,
            NewtonSolverParameters newtonSolverParameters, InterpolationChoice interpolationChoice)
        {
            _pricingDate = pricingDate;
            _periodicity = periodicity;
            _dayCounter = dayCounter;
            _newtonSolverParameters = newtonSolverParameters;
            _interpolationChoice = interpolationChoice;
        }

        public override string ToString()
        {
            return string.Format("Pricing date = {0}, Periodicity = {1}, Day counter = {2}," +
                " NewtonSolver parameters = " + _newtonSolverParameters.ToString() + ", Interpolation choice = {3}",
                _pricingDate, _periodicity, _dayCounter, _interpolationChoice);
        }
    }
}
