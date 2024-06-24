using MathematicalFunctions;

namespace Tests.MathematicalFunctions
{
    internal class DerivativeTests
    {
        [Test]
        public void LinearDerivative()
        {
            var x1 = 5;
            var x2 = 9;
            var y1 = 3 * x1 + 7;
            var y2 = 3 * x2 + 7;

            var linearFunction = new Linear(x1, y1, x2, y2);
            var derivative = linearFunction.FirstDerivative;

            var x3 = 8.8;
            var y3 = 3;
            var y3Bis = derivative.Evaluate(x3);
            Assert.That(y3Bis, Is.EqualTo(y3));
        }
    }
}
