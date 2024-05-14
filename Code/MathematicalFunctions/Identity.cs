using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    internal class Identity : IFunction
    {
        public double Evaluate(double x)
        {
            return x;
        }
    }
}
