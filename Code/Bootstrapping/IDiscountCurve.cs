using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping
{
    internal interface IDiscountCurve
    {
        double At(Date startDate, Date endDate);
    }
}
