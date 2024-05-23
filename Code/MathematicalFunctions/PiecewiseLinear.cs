using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    // Todo: Le mieux est de la définir comme f = Sum_i f_i où f_i est linéaire pour chaque i
    // Todo: C'est mieux aussi de définir f_i(x) = a_i * x + b_i sur son segment et 0 ailleurs
    public class PiecewiseLinear : IFunction
    {
        private readonly List<Linear> intervals;

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
    }
}
