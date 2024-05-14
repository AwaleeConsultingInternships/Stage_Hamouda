using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    internal class Square : IFunction
    {
        public double Evaluate(double x)
        {
            // return Math.Pow(x, 2);
            return x * x;
        }
    }
}
