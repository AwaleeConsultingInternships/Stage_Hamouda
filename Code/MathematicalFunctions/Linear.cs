using MathematicalTools;
using System.Globalization;
using QuantitativeLibrary.Maths.Functions;
using static System.Formats.Asn1.AsnWriter;

namespace MathematicalFunctions
{
    public class Linearr : RFunction
    {
        private double _x1, _y1, _x2, _y2;

        public override Linearr FirstDerivative => GetFirstDerivative();
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

        public Linearr(double x1, double y1, double x2, double y2)
        {
            if (x1 >= x2)
                throw new ArgumentException("Invalid interval");

            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }

        public Linearr(Point left, Point right)
        {
            // à compléter en utilisant Interval class et Point class
        }

        public override double Evaluate(double x)
        {
            double t = (x - _x1) / (_x2 - _x1);
            return _y1 + t * (_y2 - _y1);
        }

        protected override Linearr GetFirstDerivative()
        {
            double slope = (_y2 - _y1) / (_x2 - _x1);
            return new Linearr(_x1, slope, _x2, slope);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Linear on [({0},{1}), ({2},{3})]", _x1, _y1, _x2, _y2);
        }
    }
}
