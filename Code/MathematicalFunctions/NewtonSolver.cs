using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class NewtonSolver
    {
        private const int MaxIterations = 1000;
        private const double Tolerance = 1e-7;

        public static double FindRoot(IFunction function, IFunction deriv, double initialGuess)
        {
            double x = initialGuess;
            for (int i = 0; i < MaxIterations; i++)
            {
                double fx = function.Evaluate(x);
                double dfx = deriv.Evaluate(x);

                if (Math.Abs(dfx) < Tolerance)
                    throw new Exception("Derivative is too small");

                double newX = x - fx / dfx;

                if (Math.Abs(newX - x) < Tolerance)
                    return newX;

                x = newX;
            }

            throw new Exception("Root finding did not converge");
        }
    }
}
