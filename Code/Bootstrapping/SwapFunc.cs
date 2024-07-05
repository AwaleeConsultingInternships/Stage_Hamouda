﻿using Bootstrapping.CurveParameters;
using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Time;

namespace Bootstrapping
{
    public class SwapFunc
    {
        public static RFunction PriceWithVariableChoice(RFunction price, double deltaTotal, Parameters parameters)
        {
            switch (parameters.VariableChoice)
            {
                case VariableChoice.Discount:
                    return price;
                case VariableChoice.Yield:
                    return Composite.Create(price, new Exp(-deltaTotal));
                default:
                    throw new ArgumentException("Unknown variable choice. Found: " + parameters.VariableChoice);
            }
        }

        public static RFunction SwapValue(Dictionary<Date, RFunction> ZeroCoupons, double swapRate, Parameters parameters)
        {
            var pricingDate = parameters.PricingDate;
            var counter = parameters.DayCounter;
            var periodicity = parameters.Periodicity;

            Date datePrevious = pricingDate;

            RFunction fixedLeg = 0;
            RFunction floatingLeg = 0;

            double delta;
            double deltaTotal =0;
            RFunction P = 1;

            foreach (var date in ZeroCoupons.Keys) 
            {
                delta = counter.YearFraction(datePrevious, date);
                deltaTotal += delta;
                var RR = P;

                P = ZeroCoupons[date];
                fixedLeg = fixedLeg + delta * swapRate * P;

                var floatRate = (RR / P - 1) / delta;
                floatingLeg = floatingLeg + delta * floatRate * P;

                datePrevious = date;
            }

            var dateF = datePrevious.Advance(periodicity);
            delta = counter.YearFraction(datePrevious, dateF);
            deltaTotal += delta;

            fixedLeg = fixedLeg + delta * swapRate * Identity.Instance;

            var lastFloatRate = (P * Inverse.Instance - 1) / delta;
            floatingLeg = floatingLeg + delta * lastFloatRate * Identity.Instance;

            var price = fixedLeg - floatingLeg;
            return PriceWithVariableChoice(price, deltaTotal, parameters);
            }
    }
}
