using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantitativeLibrary.Maths.Functions;

namespace Bootstrapping.InterpolationMethods
{
    public interface Interpolator
    {
        public RFunction Compute(List<double> yields);
    }
}
