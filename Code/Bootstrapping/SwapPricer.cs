﻿using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public static class SwapPricer 
    {
        public static double Pricer(Period maturity, Discount discount, BootstrappingParameters bootstappingParameters)
        {
            double denum = 0;
            var f = bootstappingParameters.Periodicity.NbUnit;
            var m = Utilities.ConvertYearsToMonths(maturity).NbUnit;
            var pricingDate = bootstappingParameters.PricingDate;

            for (int i = 1; i <= m/f; i++)
            {
                Period pi = new Period(i * f, bootstappingParameters.Periodicity.Unit);
                double t = Utilities.Duration(pi, pricingDate, bootstappingParameters.DayCounter);
                double Bi = discount.Evaluate(t); //add evaluate with input Periodicity ?
                double delta = Utilities.Duration(bootstappingParameters.Periodicity, pricingDate, bootstappingParameters.DayCounter);
                denum += Bi * delta;
            }

            double T = Utilities.Duration(maturity, pricingDate, bootstappingParameters.DayCounter);
            double BT = discount.Evaluate(T);
            return (1-BT)/denum;
        }
    }
}
