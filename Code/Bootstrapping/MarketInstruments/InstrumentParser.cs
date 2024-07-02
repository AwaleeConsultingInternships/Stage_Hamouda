using Bootstrapping.Instruments;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Bootstrapping.MarketInstruments
{
    public static class InstrumentParser
    {
        public delegate KeyValuePair<TKey, double> InstrumentProcessor<TKey>(Instrument instrument);

        public static Dictionary<TKey, double> GetRates<TKey>(Instrument[] instruments, InstrumentProcessor<TKey> processor)
        {
            var rates = new Dictionary<TKey, double>();

            foreach (var instrument in instruments)
            {
                var result = processor(instrument);
                rates.Add(result.Key, result.Value);
            }

            return rates;
        }

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

        public static KeyValuePair<Period, double> ProcessSwap(Instrument instrument)
        {
            Period p = GetMaturity(instrument.Maturity);
            return new KeyValuePair<Period, double>(p, instrument.Value);
        }

        public static Dictionary<Period, double> GetSwapRates(Instrument[] swaps)
        {
            return GetRates(swaps, ProcessSwap);
        }

        public static KeyValuePair<Date, double> ProcessFuture(Instrument instrument)
        {
            Date p = GetThirdWednesday(instrument.Maturity);
            return new KeyValuePair<Date, double>(p, 1 - instrument.Value / 100);
        }

        public static Date GetThirdWednesday(string maturity)
        {
            string[] parts = maturity.Split(' ');
            int month = (int)(Month)Enum.Parse(typeof(Month), parts[0], true);
            int year = int.Parse(parts[1]);

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int daysToFirstWednesday = ((int)DayOfWeek.Wednesday - (int)firstDayOfMonth.DayOfWeek + 7) % 7;

            Date firstDay = new Date(1, month, year);

            Date thirdWednesday = firstDay.Advance(new Period(14 + daysToFirstWednesday, Unit.Days));
            return thirdWednesday;
        }

        public static Dictionary<Date, double> GetFutureRates(Instrument[] futures)
        {
            return GetRates(futures, ProcessFuture);
        }
    }
}
