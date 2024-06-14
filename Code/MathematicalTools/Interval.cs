namespace MathematicalTools
{
    /// <summary>
    /// A class that represent an interval in the real number line.
    /// </summary>
    public class Interval : IInterval
    {
        private double _left;
        private double _right;
        private bool _isLeftIncluded;
        private bool _isRightIncluded;

        public double Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public double Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public bool IsLeftIncluded
        {
            get { return _isLeftIncluded; }
            set { _isLeftIncluded = value; }
        }

        public bool IsRightIncluded
        {
            get { return _isRightIncluded; }
            set { _isRightIncluded = value; }
        }

        /// <summary>
        /// Creates an interval.
        /// </summary>
        /// <param name="left"> The left bound of the interval.</param>
        /// <param name="right"> The right bound of the interval. </param>
        /// <param name="isLeftIncluded"> True if the left bound is included. False if not. </param>
        /// <param name="isRightIncluded"> True if the right bound is included. False if not. </param>
        public Interval(double left, double right, bool isLeftIncluded = true, bool isRightIncluded = false) 
        {
            _left = left;
            _right = right;
            _isLeftIncluded = isLeftIncluded;
            _isRightIncluded = isRightIncluded;
        }

        /// <summary>
        /// Creates a closed interval.
        /// </summary>
        /// <param name="left"> The left bound of the interval.</param>
        /// <param name="right"> The right bound of the interval. </param>
        /// <returns></returns>
        public Interval CreateClosedInterval(double left, double right)
        {
            return new Interval(left, right, true, true);
        }

        /// <summary>
        /// True if the interal contains x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool Contains(double x)
        {
            if (_isLeftIncluded)
            {
                if(_isRightIncluded)
                {
                    return (x <= _left || x >= _right);
                }
                else
                {
                    return (x <= _left || x > _right);
                }
            }
            else
            {
                if (_isRightIncluded)
                {
                    return (x < _left || x >= _right);
                }
                else
                {
                    return (x < _left || x > _right);
                }
            }
        }
    }
}
