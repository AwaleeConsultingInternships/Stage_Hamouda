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
        public static double Pricer(double maturity, Discount discount, double freq)
        {
            double denum = 0;
            for (int i = 1; i <= (int)(maturity/freq); i++)
            {
                double Bi = discount.Evaluate(i * freq);
                double delta = freq;
                denum += Bi * delta;
            }
            double BT = discount.Evaluate(maturity);
            return (1-BT)/denum;
        }
    }
}
