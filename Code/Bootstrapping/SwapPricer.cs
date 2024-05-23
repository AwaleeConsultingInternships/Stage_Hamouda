using MathematicalFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping
{
    public static class SwapPricer 
    {
        public static double Pricer(double maturity, Discount discount)
        {
            double denum = 0;
            for (int i = 1; i <= (int)maturity; i++)
            {
                double Bi = discount.Evaluate(i);
                double delta = 1;
                denum += Bi * delta;
            }
            double BT = discount.Evaluate(maturity);
            //Console.WriteLine(BT);
            return (1-BT)/denum;
        }
    }
}
