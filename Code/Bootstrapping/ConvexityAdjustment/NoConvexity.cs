using Bootstrapping.CurveParameters;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping.ConvexityAdjustment
{
    public class NoConvexity : CAModel
    {

        public Dictionary<Date, double> AdjustConvexity(Dictionary<Date, double> futureRates)
        {
            return futureRates;
        }
    }
}
