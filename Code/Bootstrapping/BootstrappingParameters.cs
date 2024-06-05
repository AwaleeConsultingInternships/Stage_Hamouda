using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class BootstrappingParameters
    {
        private Date _pricingDate;
        private Period _periodicity;
        private DayCounter _dayCounter;

        public Date PricingDate
        {
            get { return _pricingDate; }
            set { _pricingDate = value; }
        }

        public Period Periodicity
        {
            get { return _periodicity; }
            set { _periodicity = value; }
        }

        public DayCounter DayCounter
        {
            get { return _dayCounter; }
            set { _dayCounter = value; }
        }

        public BootstrappingParameters(Date pricingDate, Period period, DayCounter dayCounter)
        {
            _pricingDate = pricingDate;
            _periodicity = period;
            _dayCounter = dayCounter;
        }

        public override string ToString()
        {
            return string.Format("Pricing date = {0}, Periodicity = {1}, Day counter = {2}", _pricingDate, _periodicity, _dayCounter);
        }
    }
}
