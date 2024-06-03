namespace MathematicalTools
{
    /// <summary>
    /// A class that represent a point in a two-dimensional plan.
    /// </summary>
    public class Point
    {
        private double _x;
        private double _y;

        public double X {
            get { return _x; }
            set { _x = value; }
        }

        public double Y {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Creates a Point with two coordinates.
        /// </summary>
        /// <param name="x"> The abscissa of the point. </param>
        /// <param name="y"> The ordinate of the point. </param>
        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return "(x = " + _x + ", " + "y = " + _y + ")";
        }
    }
}