namespace MathematicalTools
{
    /// <summary>
    /// A class that represent a segment delimited by two points.
    /// </summary>
    public class Segment
    {
        private Point _left;
        private Point _right;

        public Point Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public Point Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Creates a segment with two points.
        /// </summary>
        /// <param name="left"> The left bound of the segment. </param>
        /// <param name="right"> The right bound of the segment. </param>
        public Segment(Point left, Point right)
        {
            _left = left;
            _right = right;
        }
    }
}
