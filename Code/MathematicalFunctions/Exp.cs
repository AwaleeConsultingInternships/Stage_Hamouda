using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class Exp : RFunction
    {
        private double _a;

        public override RFunction FirstDerivative => GetFirstDerivative();

        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        public Exp(double a)
        {
            _a = a;
        }

        public override double Evaluate(double x)
        {
            return Math.Exp(_a * x);
        }

        protected override RFunction GetFirstDerivative()
        {
            return new FuncMult(new ConstantFunction(_a), new Exp(_a));
        }
    }
}
