using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapping
{
    public class SwapFunc
    {
        public static RFunction SwapValue(Dictionary<Period, double> ZCDict, double swapRate, BootstrappingParameters bootstrappedParameters)
        {
            var pricingDate = bootstrappedParameters.PricingDate;
            var counter = bootstrappedParameters.DayCounter;
            var periodicity = bootstrappedParameters.Periodicity;

            Date datePrevious;
            Date date;

            RFunction fixedLeg = new AffineFunction(0, 0);
            RFunction floatingLeg = new AffineFunction(0, 0);

            double delta;
            double deltaTotal;
            double P = 1;

            foreach (var period in ZCDict.Keys) 
            {
                datePrevious = pricingDate.Advance(period);
                date = datePrevious.Advance(periodicity);
                delta = counter.YearFraction(datePrevious, date);
                deltaTotal = counter.YearFraction(pricingDate, date);

                var RR = P;

                P = ZCDict[period];
                fixedLeg = new FuncSum(fixedLeg, new AffineFunction(swapRate * delta * P, 0));

                RR = (RR / P - 1) / delta;
                floatingLeg = new FuncSum(floatingLeg, new AffineFunction(RR * delta * P, 0));
            }
            datePrevious = pricingDate.Advance(periodicity);
            date = datePrevious.Advance(periodicity);
            delta = counter.YearFraction(datePrevious, date);

            fixedLeg = new FuncSum(fixedLeg, new AffineFunction(0, swapRate * delta));

            RFunction R = new FuncMult(new ConstantFunction(1 / delta), new FuncSum(new FuncMult(new ConstantFunction(P), new Inverse()), new ConstantFunction(-1)));
            floatingLeg = new FuncSum(floatingLeg, new FuncMult(R, new AffineFunction(0, delta)));

            return new FuncSum(fixedLeg, new FuncMult(new ConstantFunction(-1), floatingLeg));
        }
    }
}
