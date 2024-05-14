using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    internal class Composite : IFunction
    {
        // declaration des variables
        private IFunction f;

        private IFunction g;

        // récuparation des variables à l'aide de propriétés
        public IFunction F { get; set; }
        public IFunction G { get; set; }

        //une propriété ? implémenter des Getters et des setters en utilisant des propriétés

        public Composite(IFunction f, IFunction g)
        {
            F = f;
            G = g;
        }

        public double Evaluate(double x)
        {
            double gx = G.Evaluate(x);
            return F.Evaluate(gx);
        }
    }
}
