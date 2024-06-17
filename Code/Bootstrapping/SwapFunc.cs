using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class SwapFunc
    {
        public static RFunction SwapValue(Dictionary<Period, double> ZCDict, double swapRate, BootstrappingParameters bootstrappedParameters)
        {
            var pricingDate = bootstrappedParameters.PricingDate;
            var counter = bootstrappedParameters.DayCounter;
            var periodicity = bootstrappedParameters.Periodicity;

            Date datePrevious = pricingDate;
            Date date;

            RFunction fixedLeg = new AffineFunction(0, 0);
            RFunction floatingLeg = new AffineFunction(0, 0);

            double delta;
            double deltaTotal;
            double P = 1;

            foreach (var period in ZCDict.Keys) 
            {
                date = pricingDate.Advance(period);
                delta = counter.YearFraction(datePrevious, date);
                deltaTotal = counter.YearFraction(pricingDate, date);

                var RR = P;

                P = ZCDict[period];
                fixedLeg = fixedLeg + new AffineFunction(swapRate * delta * P, 0);

                RR = (RR / P - 1) / delta;
                floatingLeg = floatingLeg + new AffineFunction(RR * delta * P, 0);

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
