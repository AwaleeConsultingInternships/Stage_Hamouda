using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time
{
    public class Time
    {
        public static DateTime ConvertToDate(string dateString)
        {
            return DateTime.ParseExact(dateString, "dd/MM/yyyy", null);
        }

        public static double GetMaturity(DateTime date)
        {
            DateTime today = DateTime.Today;
            return (date - today).TotalDays / 360;     //360 days in a year convention
        }

        public static double GetMaturity(string maturity)
        {
            if (maturity.EndsWith("Y"))
            {
                return double.Parse(maturity.Substring(0, maturity.Length - 1));
            }
            else if (maturity.EndsWith("M"))
            {
                return double.Parse(maturity.Substring(0, maturity.Length - 1)) / 12;
            }
            else if (maturity.EndsWith("D"))
            {
                return double.Parse(maturity.Substring(0, maturity.Length - 1)) / 360;
            }
            else
            {
                throw new ArgumentException("Invalid period format. Expected format is '1Y', '2M', or '30D'.");
            }
        }
    }
}
