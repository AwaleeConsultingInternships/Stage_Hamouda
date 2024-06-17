using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Square : RFunction
    {
        public override RFunction FirstDerivative => GetFirstDerivative();
        public override double Evaluate(double x)
        {
            // return Math.Pow(x, 2);
            return x * x;
        }

        protected override RFunction GetFirstDerivative()
        {
            return new AffineFunction(0, 2);
        }
    }
}
