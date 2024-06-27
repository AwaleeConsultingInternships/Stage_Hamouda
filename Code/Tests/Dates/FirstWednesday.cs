using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bootstrapping;
using QuantitativeLibrary.Time;

namespace Tests.Dates
{
    public class FirstWednesday
    {
        [Test]
        public void FirstWednesdayTest()
        {
            Date x = Bootstrapping.Utilities.GetThirdWednesday("JUN 2025");
            string y = "18/6/2025";
            Assert.That(x.ToString, Is.EqualTo(y));
        }
    }
}
