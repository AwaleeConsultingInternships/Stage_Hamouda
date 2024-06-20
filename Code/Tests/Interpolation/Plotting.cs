﻿using Bootstrapping;
using Interpolation;
using MathematicalTools;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using Utilities;
using static QuantitativeLibrary.Time.Time;

namespace Tests.Interpolation
{
    internal class Plotting
    {
        [Test]
        public void Test1()
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            RootObject deserializedObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(12, Unit.Months);
            var dayCouner = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingRawData;
            var dataChoice = DataChoice.RawData;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCouner, newtonSolverParameters, interpolationChoice, dataChoice);

            var swapRates = Bootstrapping.Utilities.GetSwapRates(deserializedObject.swaps);

            var algorithm = new Algorithm(bootstrappingParameters);
            var discount = algorithm.Curve(swapRates);

            var nbYears = 40;
            var shift = 21;
            var list = new List<Point>();
            for (int i = 0; i < 365 * nbYears; i+= shift)
            {
                list.Add(new Point(i / 365.0, discount.Evaluate(i / 365.0)));
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "chart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
    }
}
