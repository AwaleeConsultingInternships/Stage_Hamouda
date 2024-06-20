using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class SwapFunc
    {
        public static RFunction SwapValue(Dictionary<Period, RFunction> ZCDict, double swapRate, Parameters parameters)
        {
            var pricingDate = parameters.PricingDate;
            var counter = parameters.DayCounter;
            var periodicity = parameters.Periodicity;

            Date datePrevious = pricingDate;
            Date date;

            RFunction fixedLeg = new AffineFunction(0, 0);
            RFunction floatingLeg = new AffineFunction(0, 0);

            double delta;
            double deltaTotal;
            RFunction P = new ConstantFunction(1);

            foreach (var period in ZCDict.Keys) 
            {
                date = pricingDate.Advance(period);
                delta = counter.YearFraction(datePrevious, date);
                deltaTotal = counter.YearFraction(pricingDate, date);

                var RR = P;

                P = ZCDict[period];
                fixedLeg = fixedLeg + swapRate * delta * P;

                RR = (RR / P - 1) / delta;
                floatingLeg = floatingLeg + delta * RR * P;

                datePrevious = date;
            }

            //datePrevious = pricingDate.Advance(periodicity);
            date = datePrevious.Advance(periodicity);
            delta = counter.YearFraction(datePrevious, date);

            fixedLeg = fixedLeg + new AffineFunction(0, swapRate * delta);

            RFunction R = 1/delta * (P * new Inverse() -1);
            floatingLeg = floatingLeg + R * new AffineFunction(0, delta);

            //floatingLeg = new ConstantFunction(1) - new AffineFunction(0, 1);

            return fixedLeg - floatingLeg;
        }
    }
}
