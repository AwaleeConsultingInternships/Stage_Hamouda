using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public static class FuturePricer
    {
        public static double Pricer(Date startDate, Discount discount, Parameters parameters)
        {
            var counter = parameters.DayCounter;
            var maturity = Utilities.GetFutureMaturity(startDate);
            var delta = counter.YearFraction(startDate, maturity);
            var P1 = discount.At(startDate);
            var P2 = discount.At(maturity);
            var price =  (P1 / P2 - 1) / delta;

            return price;
        }
    }
}
