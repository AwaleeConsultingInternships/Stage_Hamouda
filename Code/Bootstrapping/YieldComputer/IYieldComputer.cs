using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Time;

namespace Bootstrapping.YieldComputer
{
    public interface IYieldComputer
    {
        //public List<double> Compute(Dictionary<Period, double> interpolatedSwapRates);

        //public List<double> Compute(Dictionary<Date, double> futureRates);
        public static double GetYield(double result, double deltaTotal, Parameters parameters)
        {
            switch (parameters.VariableChoice)
            {
                case VariableChoice.Discount:
                    return -Math.Log(result) / deltaTotal;
                case VariableChoice.Yield:
                    return result;
                default:
                    throw new ArgumentException("Unknown variable choice. Found: " + parameters.VariableChoice);
            }
        }

        public static double GetDiscount(double result, double deltaTotal, Parameters parameters)
        {
            switch (parameters.VariableChoice)
            {
                case VariableChoice.Discount:
                    return result;
                case VariableChoice.Yield:
                    return Math.Exp(-result * deltaTotal);
                default:
                    throw new ArgumentException("Unknown variable choice. Found: " + parameters.VariableChoice);
            }
        }
    }
}
