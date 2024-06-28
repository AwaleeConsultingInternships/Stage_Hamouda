using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using MathematicalFunctions;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Bootstrapping
{
    public class Utilities
    {
        public static double Duration(Period p, Date pricingDate, DayCounter counter)
        {
            Date maturityDate = pricingDate.Advance(p);
            return counter.YearFraction(pricingDate, maturityDate);
        }


        public static Period ConvertPeriodToMonths(Period period)
        {
            if (period.Unit == Unit.Years)
                return new Period(12 * period.NbUnit, Unit.Months);
            if (period.Unit == Unit.Months)
                return period;
            throw new ArgumentException("Supports only period like xY or xM.");
        }

        public static Dictionary<Period, double> InterpolateSwapRate(PiecewiseLinear SwapInt,
            Dictionary<Period, double> swapRates, Parameters bootstrappingParameters)
        {
            var pricingDate = bootstrappingParameters.PricingDate;
            var periodicity = bootstrappingParameters.Periodicity;
            var counter = bootstrappingParameters.DayCounter;

            var maturities = swapRates.Keys.ToArray();

            var lastMaturity = ConvertPeriodToMonths(swapRates.Keys.Last());
            var j = 1;
            var f = periodicity.NbUnit;

            var result = new Dictionary<Period, double>();
            while (j * f <= lastMaturity.NbUnit)
            {
                var p = new Period(j * f, periodicity.Unit);
                if (!maturities.Contains(p))
                {
                    double x = Duration(p, pricingDate, counter);
                    double sw = SwapInt.Evaluate(x);
                    result.Add(p, sw);
                }
                else
                    result.Add(p, swapRates[p]);
                j++;
            }

            result = result.OrderBy(pair => pair.Key).ToDictionary(x => x.Key, x => x.Value);
            return result;
        }
    }
}
