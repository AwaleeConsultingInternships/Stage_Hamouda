using System.Globalization;

namespace MathematicalFunctions
{
    public class PiecewiseLinear : IFunction
    {
        private List<Linear> intervals;

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

        public double Evaluate(double x)
        {
            double s = 0;
            foreach (Linear interval in intervals) 
            {
                s += interval.Evaluate(x); 
            }
            return s;
            throw new ArgumentException("x=" + x + " is out of the defined intervals.");
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
