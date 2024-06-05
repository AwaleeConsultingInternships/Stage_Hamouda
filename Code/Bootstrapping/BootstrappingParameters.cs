using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class BootstrappingParameters
    {
        private Date _pricingDate;
        private Period _period;
        private DayCounter _dayCounter;

        public Date PricingDate
        {
            get { return _pricingDate; }
            set { _pricingDate = value; }
        }

        public Period Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public DayCounter DayCounter
        {
            get { return _dayCounter; }
            set { _dayCounter = value; }
        }

        public BootstrappingParameters(Date pricingDate, Period period, DayCounter dayCounter)
        {
            _pricingDate = pricingDate;
            _period = period;
            _dayCounter = dayCounter;
        }
    }
}
