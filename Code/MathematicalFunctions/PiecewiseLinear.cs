using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.RealInterval;

namespace MathematicalFunctions
{
    public class PiecewiseLinear : PiecewiseFunction<AffineFunction>
    {
        public void AddInterval(double x1, double y1, double x2, double y2)
        {
            var isContained = x2.Equals(double.PositiveInfinity)
                ? false
                : true;
            pieces.Add(new RInterval(x1, x2, true, isContained), AffineFunction.Create(x1, x2, y1, y2));
        }
    }
}
