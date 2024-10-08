﻿using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using static QuantitativeLibrary.Time.Time;

namespace Tests.BootstrappingTests
{
    public class Repricing
    {
        public static Array GetNumberOfMonths()
        {
            return new int[] { 3, 6, 12 };
        }

        public static Array GetInterpolationChoices()
        {
            return new[] { InterpolationChoice.UsingDirectSolving, InterpolationChoice.UsingNewtonSolver };
        }

        public static Array GetVariableChoices()
        {
            return Enum.GetValues(typeof(VariableChoice));
        }

        public static Array GetInterpolationMethods()
        {
            return Enum.GetValues(typeof(InterpolationMethod));
        }

        [Test]
        public void InterpolatedData([ValueSource(nameof(GetNumberOfMonths))] int numberOfMonth,
            [ValueSource(nameof(GetInterpolationChoices))] InterpolationChoice interpolationChoice,
            [ValueSource(nameof(GetInterpolationMethods))] InterpolationMethod interpolationMethod,
            [ValueSource(nameof(GetVariableChoices))] VariableChoice variableChoice)
        {
            var jsonContent = Utilities.Directories.GetJsonContent();
            var deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.Swaps);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(numberOfMonth, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var dataChoice = DataChoice.InterpolatedData;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            Algorithm algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            foreach (var swap in swapRates)
            {
                var maturity = swap.Key;
                var price = SwapPricer.Pricer(maturity, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-6));
            }
        }

        [Test]
        public void RawData([ValueSource(nameof(GetNumberOfMonths))] int numberOfMonth,
            [ValueSource(nameof(GetInterpolationMethods))] InterpolationMethod interpolationMethod,
            [ValueSource(nameof(GetVariableChoices))] VariableChoice variableChoice)
        {
            var jsonContent = Utilities.Directories.GetJsonContent();
            var deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.Swaps);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(numberOfMonth, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var dataChoice = DataChoice.RawData;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            Algorithm algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            foreach (var swap in swapRates)
            {
                var maturity = swap.Key;
                var price = SwapPricer.Pricer(maturity, discount, bootstrappingParameters);
                Assert.That(price, Is.EqualTo(swapRates[maturity]).Within(1e-8));
            }
        }
    }
}
