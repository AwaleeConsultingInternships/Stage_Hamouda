using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantitativeLibrary.Maths.Functions;
using static System.Formats.Asn1.AsnWriter;

namespace MathematicalFunctions
{
    public class Composite : RFunction
    {
        // declaration des variables
        private RFunction f;

        private RFunction g;

        public override RFunction FirstDerivative => GetFirstDerivative();

        // récuparation des variables à l'aide de propriétés
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

        public Composite(RFunction ff, RFunction gg)
        {
            f = ff;
            g = gg;
        }

        public override double Evaluate(double x)
        {
            double gx = g.Evaluate(x);
            return f.Evaluate(gx);
        }

        protected override RFunction GetFirstDerivative()
        {
            return new FuncMult(new Composite(f.FirstDerivative, g), g.FirstDerivative);
        }

        public override string ToString()
        {
            return $"Composite function: f(g(x)), where f = {f.ToString}, g = {g.ToString}";
        }
    }
}
