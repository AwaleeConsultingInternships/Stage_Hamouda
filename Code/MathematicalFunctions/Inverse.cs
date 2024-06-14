using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return new Composite(new FuncMult(new ConstantFunction(-1), new Inverse()), new Square());
        }
    }
}
