using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public interface IYieldComputer
    {
        public List<double> Compute(Period lastMaturity, Dictionary<Period, double> interpolatedSwapRates);
    }
}
