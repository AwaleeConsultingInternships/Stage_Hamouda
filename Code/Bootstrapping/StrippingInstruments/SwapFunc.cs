using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping.StrippingInstruments
{
    public class SwapFunc
    {
        public static RFunction SwapValue(Dictionary<Date, RFunction> ZeroCoupons,
            KeyValuePair<Date, double> swap, Parameters parameters)
        {
            var pricingDate = parameters.PricingDate;
            var counter = parameters.DayCounter;
            var periodicity = parameters.Periodicity;

            var swapRate = swap.Value;

            Date datePrevious = pricingDate;

            RFunction fixedLeg = 0;
            RFunction floatingLeg = 0;

            double delta;
            double deltaTotal = 0;
            RFunction P = 1;

            foreach (var date in ZeroCoupons.Keys)
            {
                delta = counter.YearFraction(datePrevious, date);
                deltaTotal += delta;
                var RR = P;

                P = ZeroCoupons[date];
                fixedLeg = fixedLeg + delta * swapRate * P;

                var floatRate = (RR / P - 1) / delta;
                floatingLeg = floatingLeg + delta * floatRate * P;

                datePrevious = date;
            }

            var dateF = datePrevious.Advance(periodicity);
            delta = counter.YearFraction(datePrevious, dateF);
            deltaTotal += delta;

            fixedLeg = fixedLeg + delta * swapRate * Identity.Instance;

            var lastFloatRate = (P * Inverse.Instance - 1) / delta;
            floatingLeg = floatingLeg + delta * lastFloatRate * Identity.Instance;

            var price = fixedLeg - floatingLeg;
            return Utilities.PriceWithVariableChoice(price, deltaTotal, parameters);
        }
    }
}
