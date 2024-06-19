using MathematicalFunctions;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Bootstrapping
{
    public class Utilities
    {
        public static Period GetMaturity(string maturity)
        {
            var mat = int.Parse(maturity.Substring(0, maturity.Length - 1));
            var unit = maturity[maturity.Length - 1];
            switch (unit)
            {
                case 'Y':
                    return new Period(mat, Unit.Years);
                case 'M':
                    return new Period(mat, Unit.Months);
                case 'D':
                    return new Period(mat, Unit.Days);
                case 'W':
                    return new Period(mat, Unit.Weeks);
                default:
                    throw new ArgumentException("Invalid Periodicity format. Expected format is '1Y', '2M', or '30D'.");
            }
        }

        public static double Duration(Period p, Date pricingDate, DayCounter counter)
        {
            Date maturityDate = pricingDate.Advance(p);
            return counter.YearFraction(pricingDate, maturityDate);
        }


        public static Period ConvertYearsToMonths(Period period)
        {
            if (period.Unit == Unit.Years)
                return new Period(12 * period.NbUnit, Unit.Months);
            throw new ArgumentException("Supports only period like xY.");
        }

        public static Dictionary<Period, double> GetSwapRates(Swap[] swaps)
        {
            var swapRates = new Dictionary<Period, double>();

            foreach (var swap in swaps)
            {
                string maturity = swap.maturity;
                Period p = GetMaturity(maturity);
                swapRates.Add(p, swap.rate);
            }
            return swapRates;
        }

        public static Dictionary<Period, double> InterpolateSwapRate(PiecewiseLinear SwapInt,
            Dictionary<Period, double> swapRates, Parameters bootstrappingParameters)
        {
            var pricingDate = bootstrappingParameters.PricingDate;
            var periodicity = bootstrappingParameters.Periodicity;
            var counter = bootstrappingParameters.DayCounter;

            var maturities = swapRates.Keys.ToArray();

            var lastMaturity = ConvertYearsToMonths(swapRates.Keys.Last());
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
