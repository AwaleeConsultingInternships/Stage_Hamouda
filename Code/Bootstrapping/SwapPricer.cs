using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public static class SwapPricer 
    {
        public static double Pricer(Period maturity, Discount discount, Parameters bootstrappingParameters)
        {
            double denum = 0;
            var f = bootstrappingParameters.Periodicity.NbUnit;
            var m = Utilities.ConvertPeriodToMonths(maturity).NbUnit;
            var pricingDate = bootstrappingParameters.PricingDate;

            var datePrevious = pricingDate;
            var date = pricingDate.Advance(bootstrappingParameters.Periodicity);

            for (int i = 1; i <= m/f; i++)
            {
                Period pi = new Period(i * f, bootstrappingParameters.Periodicity.Unit);
                double t = Utilities.Duration(pi, pricingDate, bootstrappingParameters.DayCounter);
                double Bi = discount.Evaluate(t); //add evaluate with input Periodicity ?
                double delta = bootstrappingParameters.DayCounter.YearFraction(datePrevious, date);
                denum += Bi * delta;

                datePrevious = date;
                date = date.Advance(bootstrappingParameters.Periodicity);
            }

            double T = Utilities.Duration(maturity, pricingDate, bootstrappingParameters.DayCounter);
            double BT = discount.Evaluate(T);
            return (1-BT)/denum;
        }
    }
}
