﻿using Bootstrapping;
using Bootstrapping.CurveParameters;
using Bootstrapping.Instruments;
using Bootstrapping.MarketInstruments;
using MathematicalTools;
using Newtonsoft.Json;
using QuantitativeLibrary.Time;
using System.Diagnostics.Metrics;
using Utilities;
using static QuantitativeLibrary.Time.Time;

namespace Tests.BootstrappingTests
{
    internal class Plotting
    {
        [Test]
        public void DiscountCurve()
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

            var algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            var nbYears = 30;
            var shift = 21;
            var shiftPeriod = new Period(shift, Unit.Days);
            var list = new List<Point>();
            var date = pricingDate.Advance(new Period(15, Unit.Years));
            for (int i = 365 * 15; i < 365 * nbYears; i+= shift)
            {
                list.Add(new Point(i / 365.0, discount.At(date)));
                date = date.Advance(shiftPeriod);
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "SwapsDiscountCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }

        [Test]
        public void DiscountCurveFutures()
        {
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "futures.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var period = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            var futureRates = InstrumentParser.GetFutureRates(deserializedObject.MarketInstruments);

            var algorithm = new Algorithm(bootstrappingParameters);

            var discount = algorithm.Curve(futureRates);

            var nbYears = 3;
            var shift = 21;
            var shiftPeriod = new Period(shift, Unit.Days);
            var list = new List<Point>();
            var date = pricingDate;
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                list.Add(new Point(i / 365.0, discount.At(date)));
                date = date.Advance(shiftPeriod);
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "FuturesDiscountCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }

        [Test]
        public void ZcYieldCurve()
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
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

            var algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            var nbYears = 40;
            var shift = 21;
            var shiftPeriod = new Period(shift, Unit.Days);
            var list = new List<Point>();
            var date = pricingDate;
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                list.Add(new Point(i / 365.0, discount.ZcYieldAt(date)));
                date = date.Advance(shiftPeriod);
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "ZcYieldCurveChart.html");

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
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, period,
                dayCounter, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

            var algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            var nbYears = 40;
            var shift = 21;
            var shiftPeriod = new Period(shift, Unit.Days);
            var list = new List<Point>();
            var date = pricingDate;
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                list.Add(new Point(i / 365.0, discount.YieldAt(date)));
                date = date.Advance(shiftPeriod);
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, "YieldCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
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
            var projectDirectory = Directories.GetMarketDataDirectory();
            string marketDataFilePath = Path.Combine(projectDirectory, "Swaps.json");

            string jsonContent = File.ReadAllText(marketDataFilePath);
            Instruments deserializedObject = JsonConvert.DeserializeObject<Instruments>(jsonContent);

            var pricingDate = new Date(01, 05, 2024);
            var periodOneYear = new Period(12, Unit.Months);
            var dayCounter = new DayCounter(DayConvention.ACT365);
            var newtonSolverParameters = new NewtonSolverParameters();
            var interpolationChoice = InterpolationChoice.UsingNewtonSolver;
            var interpolationMethod = InterpolationMethod.LinearOnYield;
            var dataChoice = DataChoice.RawData;
            var variableChoice = VariableChoice.Yield;
            var bootstrappingParameters = new Parameters(pricingDate, periodOneYear,
                dayCounter, newtonSolverParameters, interpolationChoice, interpolationMethod, dataChoice, variableChoice);

            var swapRates = InstrumentParser.GetSwapRates(deserializedObject.MarketInstruments);

            var algorithm = new Algorithm(bootstrappingParameters);

            var newSwapRates = Bootstrapping.Utilities.PeriodToDate(swapRates, bootstrappingParameters);

            var discount = algorithm.Curve(newSwapRates);

            var nbYears = 40;
            var shift = 21;
            var shiftPeriod = new Period(shift, Unit.Days);
            var list = new List<Point>();
            var date = pricingDate;
            for (int i = 0; i < 365 * nbYears; i += shift)
            {
                list.Add(new Point(i / 365.0, discount.ForwardAt(period, date)));
                date = date.Advance(shiftPeriod);
            }

            ChartHtmlGenerator generator = new ChartHtmlGenerator();
            var folderPath = Directories.GetGraphDirectory();
            var resultFilePath = Path.Combine(folderPath, period + "_ForwardCurveChart.html");

            generator.WriteHtmlToFile(list, resultFilePath);
        }
    }
}
