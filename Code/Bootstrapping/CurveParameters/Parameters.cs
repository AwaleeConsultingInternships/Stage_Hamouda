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

        private InterpolationMethod _interpolationMethod;
        public InterpolationMethod InterpolationMethod
        {
            get { return _interpolationMethod; }
            set { _interpolationMethod = value; }
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
            InterpolationMethod interpolationMethod, DataChoice dataChoice, VariableChoice variableChoice)
        {
            _pricingDate = pricingDate;
            _periodicity = periodicity;
            _dayCounter = dayCounter;
            _newtonSolverParameters = newtonSolverParameters;
            _interpolationChoice = interpolationChoice;
            _dataChoice = dataChoice;
            _variableChoice = variableChoice;
            _interpolationMethod = interpolationMethod;
        }

        public override string ToString()
        {
            return string.Format("Pricing date = {0}, Periodicity = {1}, Day counter = {2}," +
                " NewtonSolver parameters = " + _newtonSolverParameters.ToString() + ", Interpolation choice = {3}"
                + ", Interpolation Method = {4}, Data choice = {5}, Variable choice = {6}",
                _pricingDate, _periodicity, _dayCounter, _interpolationChoice, _interpolationMethod, _dataChoice, _variableChoice);
        }
    }
}
