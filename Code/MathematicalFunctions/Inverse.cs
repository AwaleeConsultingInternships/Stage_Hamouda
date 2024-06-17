using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Inverse : RFunction
    {
        public override RFunction FirstDerivative => GetFirstDerivative();
        public override double Evaluate(double x)
        {
            return 1 / x;
        }

        protected override RFunction GetFirstDerivative()
        {
            return -1 / new Square();
        }

        public override string ToString()
        {
            return "1/x";
        }
    }
}
