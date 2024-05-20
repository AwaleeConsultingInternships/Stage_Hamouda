using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class Composite : IFunction
    {
        // declaration des variables
        private IFunction f;

        private IFunction g;

        // récuparation des variables à l'aide de propriétés
        public IFunction F
        {
            get { return f; }
            set { f = value; }
        }
        public IFunction G 
        {
            get { return g; }
            set { g = value; }
        }

        public Composite(IFunction ff, IFunction gg)
        {
            f = ff;
            g = gg;
        }

        public double Evaluate(double x)
        {
            double gx = g.Evaluate(x);
            return f.Evaluate(gx);
        }
    }
}
