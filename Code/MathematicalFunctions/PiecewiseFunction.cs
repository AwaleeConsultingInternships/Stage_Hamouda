using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.RealInterval;

namespace MathematicalFunctions
{
    public class PiecewiseFunction<T> : RFunction
        where T : RFunction
    {
        internal Dictionary<RInterval, T> pieces;

        public override RFunction FirstDerivative => GetFirstDerivative();

        public Dictionary<RInterval, T> Pieces
        {
            get { return pieces; }
            set { pieces = value; }
        }

        public PiecewiseFunction()
        {
            pieces = new Dictionary<RInterval, T>();
        }

        public void AddPiece(RInterval rInterval, T rFunction)
        {
            pieces.Add(rInterval, rFunction);
        }

        public override double Evaluate(double x)
        {
            foreach (var piece in pieces)
            {
                if (piece.Key.Contains(x))
                    return piece.Value.Evaluate(x);
            }

            throw new ArgumentException("x=" + x + " is out of the defined pieces.");
        }

        protected override RFunction GetFirstDerivative()
        {
            RFunction result = new ConstantFunction(0);
            foreach (var piece in pieces)
            {
                result = result + piece.Value.FirstDerivative;
            }
            return result;
        }

        public override string ToString()
        {
            var firstPiece = pieces.First();
            var lastPiece = pieces.Last();
            var interval = new RInterval(firstPiece.Key.Inf, lastPiece.Key.Sup,
                firstPiece.Key.IsInfContained, firstPiece.Key.IsSupContained);
            return "Piecewise function on " + interval.ToString();
        }
    }
}
