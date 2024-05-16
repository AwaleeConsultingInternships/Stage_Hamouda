using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    public class Piecewise_Linear : IFunction
    {
        private readonly List<Linear> intervals;

        public Piecewise_Linear()
        {
            intervals = new List<Linear>();
        }

        public void AddInterval(double x1, double y1, double x2, double y2)
        {
            intervals.Add(new Linear(x1, y1, x2, y2));
        }

        public double Evaluate(double x)
        {
            foreach (Linear interval in intervals) 
            {
                try
                {
                    return interval.Evaluate(x);
                }
                catch (ArgumentException) 
                {
                    // Ignore
                }
            }

            throw new ArgumentException("x is out of the defined intervals");
        }
    }
}
