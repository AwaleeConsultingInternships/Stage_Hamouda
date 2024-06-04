using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping
{
    public class PeriodConventioned
    {
        public Period period { get; set; }
        public DayCounter dayCounter { get; set; }

        public PeriodConventioned(Period period, DayCounter dayCounter)
        {
            this.period = period;
            this.dayCounter = dayCounter;
        }
    }
}
