using QuantitativeLibrary.Maths.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class FuncMult : RFunction
    {
        private RFunction f;
        private RFunction g;

        public override RFunction FirstDerivative => GetFirstDerivative();

        public RFunction F
        {
            get { return f; }
            set { f = value; }
        }
        public RFunction G
        {
            get { return g; }
            set { g = value; }
        }

        public FuncMult(RFunction ff, RFunction gg)
        {
            f = ff;
            g = gg;
        }

        public override double Evaluate(double x)
        {
            return f.Evaluate(x) * g.Evaluate(x);
        }



        protected override RFunction GetFirstDerivative()
        {
            return new FuncSum(new FuncMult(f, g.FirstDerivative), new FuncMult(g, f.FirstDerivative));
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Product of ({0}) and ({1})", f.ToString(), g.ToString());
        }

        /*public static RFunction operator *(RFunction f, RFunction g)
        {
            return new FuncMult(f, g);
        }*/
    }
}
