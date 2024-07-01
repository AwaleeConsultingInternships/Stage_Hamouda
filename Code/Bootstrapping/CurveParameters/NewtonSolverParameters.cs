namespace Bootstrapping.CurveParameters
{
    public class NewtonSolverParameters
    {
        private double _target;
        public double Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private double _firstGuess;
        public double FirstGuess
        {
            get { return _firstGuess; }
            set { _firstGuess = value; }
        }

        private double _tolerance;
        public double Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }

        public NewtonSolverParameters(double target = 0,
            double firstGuess = 0.03, double tolerance = 1e-10)
        {
            _target = target;
            _firstGuess = firstGuess;
            _tolerance = tolerance;
        }

        public override string ToString()
        {
            return string.Format("Target = {0}, First guess = {1}, Tolerance = {2}",
                _target, _firstGuess, _tolerance);
        }
    }
}
