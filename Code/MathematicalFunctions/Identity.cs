using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Identity : RFunction
    {
        private Identity()
        {
        }

        public static Identity Instance => new Identity();

        public override RFunction FirstDerivative => GetFirstDerivative();

        public override double Evaluate(double x)
        {
            return x;
        }

        protected override RFunction GetFirstDerivative()
        {
            return 1;
        }

        public override string ToString()
        {
            return "x -> x";
        }
    }
}
