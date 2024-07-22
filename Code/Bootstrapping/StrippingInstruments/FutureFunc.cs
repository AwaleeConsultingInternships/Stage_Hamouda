using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping.StrippingInstruments
{
    public class FutureFunc
    {
        public static RFunction FutureValue(Date previousDate, double previousZC,
            Date date, KeyValuePair<Date, double> future, Parameters parameters)
        {
            var pricingDate = parameters.PricingDate;
            var counter = parameters.DayCounter;
            var periodicity = parameters.Periodicity;

            double delta = counter.YearFraction(previousDate, date);
            double deltaTotal = counter.YearFraction(pricingDate, date);

            RFunction LHS = future.Value;
            RFunction RHS = (previousZC * Inverse.Instance - 1) / delta;

            var price = LHS - RHS;
            return Utilities.PriceWithVariableChoice(price, deltaTotal, parameters);
        }
    }
}
