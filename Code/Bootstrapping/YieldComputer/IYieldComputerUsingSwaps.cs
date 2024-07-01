using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping.YieldComputer
{
    public interface IYieldComputerUsingSwaps : IYieldComputer
    {
        public List<double> Compute(Dictionary<Period, double> interpolatedSwapRates);
    }
}
