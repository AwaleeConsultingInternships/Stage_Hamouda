using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    internal interface IDiscount
    {
        double At(Date date);
        double YieldAt(Date date);
        double ZcYieldAt(Date date);
        double ForwardAt(Period period, Date date);
    }
}
