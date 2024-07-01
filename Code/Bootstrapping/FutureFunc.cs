using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class FutureFunc
    {

        public static RFunction FutureValue(Date previousDate, double previousZC, Date date, double futureRate, Parameters parameters)
        {
            var pricingDate = parameters.PricingDate;
            var counter = parameters.DayCounter;
            var periodicity = parameters.Periodicity;

            double delta = counter.YearFraction(previousDate, date);
            double deltaTotal = counter.YearFraction(pricingDate, date);

            RFunction LHS = futureRate;
            RFunction RHS = (previousZC * Inverse.Instance) / delta;

            var price = LHS - RHS;
            return SwapFunc.PriceWithVariableChoice(price, deltaTotal, parameters);
        }
    }
}
