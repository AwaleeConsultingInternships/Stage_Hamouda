using Bootstrapping;
using MathematicalFunctions;
using QuantitativeLibrary.Time;

namespace Tests.MathematicalFunctions
{
    public class Tests
    {
        [Test]
        public void LinearEval()
        {
            var x1 = 5;
            var x2 = 9;
            var y1 = 3 * x1 + 7;
            var y2 = 3 * x2 + 7;

            var linearFunction = new Linear(x1, y1, x2, y2);
            
            var x3 = 8.8;
            var y3 = 3 * x3 + 7;
            var y3Bis = linearFunction.Evaluate(x3);
            Assert.That(y3Bis, Is.EqualTo(y3));
        }

        [Test]
        public void PiecewiseLinearEvalOk()
        {
            var x1 = 5;
            var x2 = 9;
            var x3 = 10;
            var x4 = 17;
            var y1 = 3 * x1 + 7;
            var y2 = 3 * x2 + 7;
            var y3 = 5 * x3 - 2;
            var y4 = 5 * x4 - 2;

            var piecewiseLinearFunction = new PiecewiseLinear();
            piecewiseLinearFunction.AddInterval(x1, y1, x2, y2);
            piecewiseLinearFunction.AddInterval(x3, y3, x4, y4);

            var x5 = 8.8;
            var y5 = 3 * x5 + 7;
            var y5Bis = piecewiseLinearFunction.Evaluate(x5);
            Assert.That(y5Bis, Is.EqualTo(y5));

            var x6 = 15;
            var y6 = 5 * x6 - 2;
            var y6Bis = piecewiseLinearFunction.Evaluate(x6);
            Assert.That(y6Bis, Is.EqualTo(y6));
        }

        //[Test]
        //public void PiecewiseLinearEvalKo()
        //{
        //    var x1 = 5;
        //    var x2 = 9;
        //    var x3 = 10;
        //    var x4 = 17;
        //    var y1 = 3 * x1 + 7;
        //    var y2 = 3 * x2 + 7;
        //    var y3 = 5 * x3 - 2;
        //    var y4 = 5 * x4 - 2;

        //    var piecewiseLinearFunction = new PiecewiseLinear();
        //    piecewiseLinearFunction.AddInterval(x1, y1, x2, y2);
        //    piecewiseLinearFunction.AddInterval(x3, y3, x4, y4);

        //    var x7 = 9.5;
        //    var y7Bis = piecewiseLinearFunction.Evaluate(x7);
        //}

        [TestCase(5)]
        [TestCase(0)]
        [TestCase(-2)]
        public void DiscountFromYieldTest(double asOf)
        {
            var pricingDate = new Date(01, 05, 2024);

            var yield = Identity.Instance;
            var discount = new Discount(pricingDate, yield);

            var exp = new Exp(-1);
            var square = Square.Instance;

            var discountBis = new Composite(exp, square);

            Assert.That(discount.Evaluate(asOf), Is.EqualTo(discountBis.Evaluate(asOf)));
        }
    }
}