using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Inverse : RFunction
    {
        private Inverse()
        {
        }

        public static Inverse Instance => new Inverse();

        public override RFunction FirstDerivative => GetFirstDerivative();

        public override double Evaluate(double x)
        {
            return 1 / x;
        }

        protected override RFunction GetFirstDerivative()
        {
            return -1 / Square.Instance;
        }

        public override string ToString()
        {
            return "x -> 1/x";
        }
    }
}
