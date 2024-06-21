using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public interface IYieldComputer
    {
        public List<double> Compute(Dictionary<Period, double> interpolatedSwapRates);
    }
}
