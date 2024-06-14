using QuantitativeLibrary.Maths.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalTools;
using System.Reflection.Metadata;

namespace MathematicalFunctions
{
    public class PiecewiseConstant : RFunction
    {
        private Dictionary<Interval, ConstantFunction> intervals;

        public override PiecewiseConstant FirstDerivative => GetFirstDerivative();

        public Dictionary<Interval, ConstantFunction> Intervals
        {
            get { return intervals; }
            set { intervals = value; }
        }

        public PiecewiseConstant()
        {
            intervals = new Dictionary<Interval, ConstantFunction>();
        }

        public void AddInterval(double x1, double x2, double constant)
        {
            intervals.Add(new Interval(x1, x2), new ConstantFunction(constant));
        }

        public void AddInterval(Interval interval, double constant)
        {
            intervals.Add(interval, new ConstantFunction(constant));
        }

        public void AddInterval(double x1, double x2, ConstantFunction constant)
        {
            intervals.Add(new Interval(x1, x2), constant);
        }

        public void AddInterval(Interval interval, ConstantFunction constant)
        {
            intervals.Add(interval, constant);
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

        protected override PiecewiseConstant GetFirstDerivative()
        {
            PiecewiseConstant derivFunc = new PiecewiseConstant();
            foreach (Interval interval in intervals.Keys)
            {
                derivFunc.AddInterval(interval, 0);
            }
            return derivFunc;
        }

        public override string ToString()
        {
            var firstInterval = intervals.Keys.First();
            var lastInterval = intervals.Keys.Last();
            return string.Format(CultureInfo.InvariantCulture, "Piecewise Constant on [{0}, {1}]",
                firstInterval.Left, lastInterval.Right);
        }
    }
}
