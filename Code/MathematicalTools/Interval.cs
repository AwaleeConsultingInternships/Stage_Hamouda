using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalTools
{
    public class Interval : IInterval
    {
        private Interval(double Left, double Right, bool IsLeftIncluded, bool IsRightIncluded) 
        {
            left = Left;
            right = Right;
            isRightIncluded = IsRightIncluded;
            isLeftIncluded = IsLeftIncluded;
        }

        public Interval CreateClosedInterval(double Left, double Right)
        {
            return new Interval(Left, Right, true, true);
        }

        private readonly double left;
        private readonly double right;
        private readonly bool isLeftIncluded;
        private readonly bool isRightIncluded;

        public bool Contains(double x)
        {
            if (isLeftIncluded)
            {
                if(isRightIncluded)
                {
                    return (x <= left || x >= right);
                }
                else
                {
                    return (x <= left || x > right);
                }
            }
            else
            {
                if (isRightIncluded)
                {
                    return (x < left || x >= right);
                }
                else
                {
                    return (x < left || x > right);
                }
            }
        }
    }
}
