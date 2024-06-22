using QuantitativeLibrary.Time;

namespace Bootstrapping.CurveParameters
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

        private DataChoice _dataChoice;
        public DataChoice DataChoice
        {
            get { return _dataChoice; }
            set { _dataChoice = value; }
        }

        private VariableChoice _variableChoice;
        public VariableChoice VariableChoice
        {
            get { return _variableChoice; }
            set { _variableChoice = value; }
        }

        public Parameters(Date pricingDate, Period periodicity, DayCounter dayCounter,
            NewtonSolverParameters newtonSolverParameters, InterpolationChoice interpolationChoice,
            DataChoice dataChoice, VariableChoice variableChoice)
        {
            _pricingDate = pricingDate;
            _periodicity = periodicity;
            _dayCounter = dayCounter;
            _newtonSolverParameters = newtonSolverParameters;
            _interpolationChoice = interpolationChoice;
            _dataChoice = dataChoice;
            _variableChoice = variableChoice;
        }

        public override string ToString()
        {
            return string.Format("Pricing date = {0}, Periodicity = {1}, Day counter = {2}," +
                " NewtonSolver parameters = " + _newtonSolverParameters.ToString() + ", Interpolation choice = {3}"
                + ", Data choice = {4}, Variable choice = {5}",
                _pricingDate, _periodicity, _dayCounter, _interpolationChoice, _dataChoice, _variableChoice);
        }
    }
}
