using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Square : RFunction
    {
        private Square()
        {
        }

        public static Square Instance => new Square();

        public override RFunction FirstDerivative => GetFirstDerivative();

        public override double Evaluate(double x)
        {
            return x * x;
        }

        protected override RFunction GetFirstDerivative()
        {
            return new AffineFunction(0, 2);
        }

        public override string ToString()
        {
            return "x -> x^2";
        }
    }
}
