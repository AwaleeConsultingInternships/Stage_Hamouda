using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Exp : RFunction
    {
        private double _a;
        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        private double _b;
        public double B
        {
            get { return _b; }
            set { _b = value; }
        }

        private double _c;
        public double C
        {
            get { return _c; }
            set { _c = value; }
        }

        public override RFunction FirstDerivative => GetFirstDerivative();

        public Exp(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public Exp(double a)
        {
            _a = a;
            _b = 0;
            _c = 1;
        }

        public override double Evaluate(double x)
        {
            return _c * Math.Exp(_a * (x - _b));
        }

        protected override RFunction GetFirstDerivative()
        {
            return _a * new Exp(_a, _b, _c);
        }

        public override string ToString()
        {
            return "x -> " + _c.ToString() + "*Exp(" + _a.ToString() + "*(x - " + _b.ToString() + "))";
        }
    }
}
