using QuantitativeLibrary.Time;

namespace Bootstrapping.YieldComputer
{
    public interface IYieldComputer
    {
        public List<double> Compute(Dictionary<Period, double> interpolatedSwapRates);
    }
}
