using MathematicalFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public static class SwapPricer 
    {
        public static double Pricer(Period maturity, Discount discount, PeriodConventioned freq)
        {
            double denum = 0;
            var f = freq.period.NbUnit;
            var m = maturity.NbUnit;

            for (int i = 1; i <= m/f; i++)
            {
                Period pi = new Period(i * f, freq.period.Unit);
                double t = BootstrappingClass.Duration(pi, new Date(01, 05, 2024), freq.dayCounter);
                double Bi = discount.Evaluate(t); //add evaluate with input Period ?
                double delta = BootstrappingClass.Duration(freq.period, new Date(01, 05, 2024), freq.dayCounter);
                denum += Bi * delta;
            }

            double T = BootstrappingClass.Duration(maturity, new Date(01, 05, 2024), freq.dayCounter); ;
            double BT = discount.Evaluate(T);
            return (1-BT)/denum;
        }
    }
}
