using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalFunctions
{
    internal class Linear : IFunction
    {
        private readonly double _x1, _y1, _x2, _y2;
        //On peut définir la fonction autrement, par la pente et l'interception

        public Linear(double x1, double y1, double x2, double y2)
        {
            //Ajout de condition pour que x1 < x2
            if (x1 >= x2)
                throw new ArgumentException("Invalid interval");

            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }

        public double Evaluate(double x)
        {
            //Ajout de condidtion pour que x soit dans l'intervalle
            if (x < _x1 || x > _x2)
                throw new ArgumentException("x is out of the interval");

            double t = (x - _x1) / (_x2 - _x1);
            return _y1 + t * (_y2 - _y1);
        }
    }

}
