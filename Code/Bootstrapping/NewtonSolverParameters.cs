namespace Bootstrapping
{
    public class NewtonSolverParameters
    {
        private bool _solveRoot;
        public bool SolveRoot
        {
            get { return _solveRoot; }
            set { _solveRoot = value; }
        }

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

        public NewtonSolverParameters(bool solveRoot = false, double target = 0,
            double firstGuess = 1, double tolerance = 1e-10)
        {
            _solveRoot = solveRoot;
            _target = target;
            _firstGuess = firstGuess;
            _tolerance = tolerance;
        }

        public override string ToString()
        {
            return string.Format("Solve root = {0}, Target = {1}, First guess = {2}, Tolerance = {3}",
                _solveRoot, _target, _firstGuess, _tolerance);
        }
    }
}
