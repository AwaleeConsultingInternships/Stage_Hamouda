using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using MathematicalTools;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using System.Diagnostics.Metrics;
using Utilities;
using static QuantitativeLibrary.Time.Time;

namespace Tests.Interpolation
{
    internal class Plotting
    {
        [Test]
        public void DiscountCurve()
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "Swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, dataChoice, variableChoice);

            var swapRates = Bootstrapping.Utilities.GetSwapRates(deserializedObject.Swaps);

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
            var resultFilePath = Path.Combine(folderPath, "DiscountCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
        [Test]
        public void YieldCurve()
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "Swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, dataChoice, variableChoice);

            var swapRates = Bootstrapping.Utilities.GetSwapRates(deserializedObject.Swaps);

            var algorithm = new Algorithm(bootstrappingParameters);
            var discount = algorithm.Curve(swapRates);

            var nbYears = 40;
            var shift = 21;
            var list = new List<Point>();
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                list.Add(new Point(i / 365.0, discount.Yield(i / 365.0)));
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "YieldCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ForwardCurve(int forwardMonths)
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "Swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, dataChoice, variableChoice);

            var swapRates = Bootstrapping.Utilities.GetSwapRates(deserializedObject.Swaps);

            var algorithm = new Algorithm(bootstrappingParameters);
            var discount = algorithm.Curve(swapRates);

            var p = new Period(forwardMonths, Unit.Months);
            string pString = p.ToString();
            var pForward = Bootstrapping.Utilities.Duration(p, pricingDate, dayCounter);

            var nbYears = 30;
            var shift = 21;
            var list = new List<Point>();
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                list.Add(new Point(i / 365.0, discount.Forward(i / 365.0, (i / 365.0) + pForward)));
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, pString + "ForwardCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
    }
}
