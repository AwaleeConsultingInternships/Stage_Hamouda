using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    internal interface IDiscount
    {
        double At(Date startDate, Date endDate);
    }
}
