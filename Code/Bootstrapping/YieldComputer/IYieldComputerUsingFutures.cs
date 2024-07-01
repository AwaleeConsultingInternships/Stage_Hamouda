using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping.YieldComputer
{
    public interface IYieldComputerUsingFutures : IYieldComputer
    {
        public Dictionary<Date, double> Compute(Dictionary<Date, double> futureRates);
    }
}
