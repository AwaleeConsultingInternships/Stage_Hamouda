using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Composite : RFunction
    {
        private RFunction _f;
        public RFunction F
        {
            get { return _f; }
            set { _f = value; }
        }

        private RFunction _g;
        public RFunction G
        {
            get { return _g; }
            set { _g = value; }
        }

        public Composite(RFunction f, RFunction g)
        {
            _f = f;
            _g = g;
        }

        public static RFunction Create(RFunction f, RFunction g)
        {
            if (f is Exp fExp && g is AffineFunction gAffine)
                return new Exp(fExp.A * gAffine.Slope, (fExp.B - gAffine.Origin) / gAffine.Slope, fExp.C);
            return new Composite(f, g);
        }

        public override RFunction FirstDerivative => GetFirstDerivative();

        public override double Evaluate(double x)
        {
            double gx = _g.Evaluate(x);
            return _f.Evaluate(gx);
        }

        protected override RFunction GetFirstDerivative()
        {
            return Create(_f.FirstDerivative, _g) * _g.FirstDerivative;
        }

        public override string ToString()
        {
            return "(" + _f.ToString() + ")o(" + _g.ToString() + ")";
        }
    }
}
