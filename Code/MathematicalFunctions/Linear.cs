using System;
using MathematicalTools;

namespace MathematicalFunctions
{
    public class Linear : IFunction
    {
        private double _x1, _y1, _x2, _y2;

        public double X1
        {
            get { return _x1; }
            set
            {
                if (value >= _x2)
                    throw new ArgumentException("Invalid interval");
                _x1 = value;
            }

        }

        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }

        public double X2
        {
            get { return _x2; }
            set
            {
                if (value <= _x1)
                    throw new ArgumentException("Invalid interval");
                _x2 = value;
            }
        }

        public double Y2
        {
            get { return _y2; }
            set { _y2 = value; }
        }

        public Linear(double x1, double y1, double x2, double y2)
        {
            if (x1 >= x2)
                throw new ArgumentException("Invalid interval");

            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }

        public Linear(Point left, Point right)
        {
            // à compléter en utilisant Interval class et Point class
        }

        public double Evaluate(double x)
        {
            if (x < _x1 || x >= _x2)
                return 0;

            double t = (x - _x1) / (_x2 - _x1);
            return _y1 + t * (_y2 - _y1);
        }
    }

}
