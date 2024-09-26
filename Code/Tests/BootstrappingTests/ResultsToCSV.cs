using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
using MathematicalTools;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using System.Globalization;
using static QuantitativeLibrary.Time.Time;
using System.Collections.Generic;
using System.IO;

namespace Tests.BootstrappingTests
{
    public class ResultsToCSV
    {
        public static Array GetNumberOfMonths()
        {
            return new Period[]
            {
                new Period(1, Unit.Days),
                new Period(3, Unit.Months),
                new Period(6, Unit.Months),
                new Period(12, Unit.Months)
            };
        }

        [Test]
        public void ForwardCurve([ValueSource(nameof(GetNumberOfMonths))] Period period)
        {
            var jsonContent = Utilities.Directories.GetJsonContent();
            var deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var periodOneYear = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.InterpolatedData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, periodOneYear,
                dayCounter, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.Swaps);

            var algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            var nbYears = 30;
            var shift = 21;
            var shiftPeriod = new Period(shift, Unit.Days);
            var results = new List<string> { "Time,ForwardRate" };
            var date = pricingDate;
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                var time = (i / 365.0).ToString(CultureInfo.InvariantCulture);
                var forwardRate = discount.ForwardAt(period, date).ToString(CultureInfo.InvariantCulture);
                results.Add($"{time},{forwardRate}");
                date = date.Advance(shiftPeriod);
            }

            var folderPath = Utilities.Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, period + "_ForwardCurve.csv");

            File.WriteAllLines(resultFilePath, results);
        }
    }
}
