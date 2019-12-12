using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib.GUI.Constraints
{
    class MinConstraint : Constraint
    {
        private int minPixels;

        public MinConstraint(int minPixels)
        {
            this.minPixels = minPixels;
        }

        public int ExecuteConstraint(int pixelValue)
        {
            return pixelValue < minPixels ? minPixels : pixelValue;
        }
    }
}
