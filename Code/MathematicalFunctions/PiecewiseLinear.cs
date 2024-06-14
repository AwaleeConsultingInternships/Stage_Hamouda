using System.Globalization;
using MathematicalTools;
using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class PiecewiseLinear : RFunction
    {
        private Dictionary<Interval, AffineFunction> intervals;

        public override RFunction FirstDerivative => GetFirstDerivative();

        public Dictionary<Interval, AffineFunction> Intervals
        {
            get { return intervals; }
            set { intervals = value; }
        }

        public PiecewiseLinear()
        {
            intervals = new Dictionary<Interval, AffineFunction>();
        }

        public void AddInterval(double x1, double y1, double x2, double y2)
        {
            intervals.Add(new Interval(x1, x2), AffineFunction.Create(x1, x2, y1, y2));
        }

        public void AddInterval(double x1, double x2, AffineFunction linear)
        {
            intervals.Add(new Interval(x1, x2), linear);
        }

        public void AddInterval(Interval interval, AffineFunction linear)
        {
            intervals.Add(interval, linear);
        }

        public override double Evaluate(double x)
        {
            foreach (Interval interval in intervals.Keys) 
            {
                if (x >= interval.Left && x < interval.Right)
                    return intervals[interval].Evaluate(x); 
            }

            throw new ArgumentException("x=" + x + " is out of the defined intervals.");
        }

        protected override RFunction GetFirstDerivative()
        {
            PiecewiseConstant derivFunc = new PiecewiseConstant();
            foreach (Interval interval in intervals.Keys)
            {
                derivFunc.AddInterval(interval, intervals[interval].FirstDerivative);
            }
            return derivFunc;
        }

        public override string ToString()
        {
            var firstInterval = intervals.First();
            var lastInterval = intervals.Last();
            return string.Format(CultureInfo.InvariantCulture, "Piecewise linear on [({0},{1}), ({2},{3})]",
                firstInterval.X1, firstInterval.Y1, lastInterval.X2, lastInterval.Y2);
        }
    }
}
