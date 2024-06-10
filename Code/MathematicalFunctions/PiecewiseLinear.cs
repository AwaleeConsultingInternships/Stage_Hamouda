using System.Globalization;
using QuantitativeLibrary.Maths.Functions;

namespace MathematicalFunctions
{
    public class PiecewiseLinear : RFunction
    {
        private List<Linear> intervals;

        public override RFunction FirstDerivative => GetFirstDerivative();

        public List<Linear> Intervals
        {
            get { return intervals; }
            set { intervals = value; }
        }

        public PiecewiseLinear()
        {
            intervals = new List<Linear>();
        }

        public void AddInterval(double x1, double y1, double x2, double y2)
        {
            intervals.Add(new Linear(x1, y1, x2, y2));
        }

        public void AddInterval(Linear linear)
        {
            intervals.Add(linear);
        }

        public override double Evaluate(double x)
        {
            double s = 0;
            foreach (Linear interval in intervals) 
            {
                s += interval.Evaluate(x); 
            }
            return s;
            throw new ArgumentException("x=" + x + " is out of the defined intervals.");
        }

        protected override RFunction GetFirstDerivative()
        {
            PiecewiseLinear derivFunc = new PiecewiseLinear();
            foreach (Linear interval in intervals)
            {
                derivFunc.AddInterval(interval.FirstDerivative);
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
