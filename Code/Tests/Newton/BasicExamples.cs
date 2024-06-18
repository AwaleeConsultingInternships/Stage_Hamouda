using MathematicalFunctions;
using QuantitativeLibrary.Maths.Functions;
using QuantitativeLibrary.Maths.Solver.RootFinder;

namespace Tests.Newton
{
    public class BasicExamples
    {
        [Test]
        public void Affine() 
        {
            var function = AffineFunction.Create(0, 1, 7, 10);
            double target = 0;
            double firstGuess = 0;
            double tolerance = 1e-6;
            NewtonSolver solver = NewtonSolver.CreateWithAbsolutePrecision(target, function, function.FirstDerivative, firstGuess, tolerance);
            NewtonResult result = solver.Solve();
            double xStar = result.Solution;

            var xBar = -7.0 / 3;
            Assert.That(xStar, Is.EqualTo(xBar).Within(1e-6));
        }

        [Test]
        public void Quadratic()
        {
            var f1 = AffineFunction.Create(0, 1, -6, -3);
            var f2 = new Square();
            var function = new Composite(f1, f2);

            double target = 0;
            double firstGuess = 1;
            double tolerance = 1e-6;
            NewtonSolver solver = NewtonSolver.CreateWithAbsolutePrecision(target, function, function.FirstDerivative, firstGuess, tolerance);
            NewtonResult result = solver.Solve();
            double xStar = result.Solution;

            var xBar = Math.Sqrt(2.0);
            Assert.That(xStar, Is.EqualTo(xBar).Within(1e-6));
        }

        [Test]
        public void Exp()
        {
            var f1 = new Exp(1);
            var f2 = AffineFunction.Create(0, 1, -1, 1);
            var f3 = new Composite(f1, f2);
            var f4 = new ConstantFunction(-Math.E * Math.E);
            var function = f3 + f4;

            double target = 0;
            double firstGuess = 1;
            double tolerance = 1e-6;
            NewtonSolver solver = NewtonSolver.CreateWithAbsolutePrecision(target, function, function.FirstDerivative, firstGuess, tolerance);
            NewtonResult result = solver.Solve();
            double xStar = result.Solution;

            var xBar = 3.0 / 2;
            Assert.That(xStar, Is.EqualTo(xBar).Within(1e-6));
        }
    }
}
