using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantitativeLibrary.Maths.Functions;
using static System.Formats.Asn1.AsnWriter;

namespace MathematicalFunctions
{
    public class Identity : RFunction
    {
        public override RFunction FirstDerivative => GetFirstDerivative();

        public override double Evaluate(double x)
        {
            return x;
        }
        protected override RFunction GetFirstDerivative()
        {
            return new ConstantFunction(1);
        }

        public override string ToString()
        {
            return $"Identity Function: f(x) = x";
        }
    }
}
