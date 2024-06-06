using MathematicalFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Newton
{
    public class Test1
    {
        [Test]
        public void Linear() 
        {
            var function = new Linear(0, 7, 1, 10);
            var deriv = new Linear(0, 3, 1, 3);
            var xBar = -7.0 / 3;
            var xStar = NewtonSolver.FindRoot(function, deriv, 1);
            Assert.That(xStar, Is.EqualTo(xBar));
        }
        [Test]
        public void Quadratic()
        {
            var f1 = new Linear(0, -6, 1, -3);
            var f2 = new Square();
            var function = new Composite(f1, f2);
            var deriv = new Linear(0, 0, 1, 6);
            var xBar = Math.Sqrt(2.0);
            var xStar = NewtonSolver.FindRoot(function, deriv, 1);
            Assert.That(xStar, Is.EqualTo(xBar));
        }
    }
}
