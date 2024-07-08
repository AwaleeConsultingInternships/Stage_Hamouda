using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;
using Bootstrapping.MarketInstruments;

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

        public static Dictionary<Date, double> InterpolateSwapRate(PiecewiseLinear SwapInt,
            Dictionary<Date, double> swapRates, Parameters bootstrappingParameters)
        {
            var pricingDate = bootstrappingParameters.PricingDate;
            var periodicity = bootstrappingParameters.Periodicity;
            var counter = bootstrappingParameters.DayCounter;

            var maturities = swapRates.Keys.ToArray();

            var result = new Dictionary<Date, double>();
            var orderDict = new Dictionary<double, Date>();
            var date = pricingDate.Advance(periodicity);

            while (date < swapRates.Keys.Last().Advance(new Period(1, Unit.Days)))
            {
                if (!maturities.Contains(date))
                {
                    double x = counter.YearFraction(pricingDate, date);
                    double sw = SwapInt.Evaluate(x);
                    result.Add(date, sw);
                    orderDict.Add(x, date);
                }
                else
                {
                    double x = counter.YearFraction(pricingDate, date);
                    orderDict.Add(x, date);
                    result.Add(date, swapRates[date]);
                }
                date = date.Advance(periodicity);
            }

            var sortedDurations = orderDict.OrderBy(pair => pair.Key);
            var orderedResult = sortedDurations.Select(pair => new KeyValuePair<Date, double>(pair.Value, result[pair.Value])).ToDictionary(x => x.Key, x => x.Value);
            return orderedResult;
        }

        public static Date GetFutureMaturity(Date startDate)
        {
            var month = startDate.Month;
            var year = startDate.Year;
            switch (month)
            {
                case 3:
                    return InstrumentParser.GetThirdWednesday("JUN " + year);

                case 6:
                    return InstrumentParser.GetThirdWednesday("SEP " + year);

                case 9:
                    return InstrumentParser.GetThirdWednesday("DEC " + year);

                case 12:
                    year++;
                    return InstrumentParser.GetThirdWednesday("MAR " + year);
            }
            return startDate;
        }

        public static Dictionary<Date, double> PeriodToDate(Dictionary<Period, double> rates, Parameters parameters)
        {
            var pricingDate = parameters.PricingDate;
            var periodicity = parameters.Periodicity;
            var counter = parameters.DayCounter;

            var result = new Dictionary<Date, double>();

            foreach ( var pair in rates)
            {
                var date = pricingDate.Advance(pair.Key);
                result.Add(date, pair.Value);
            }

            return result;
        }
    }
}
