using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class MaxConstraint : Constraint
    {
        private int maxPixels;

        public MaxConstraint(int maxPixels)
        {
            this.maxPixels = maxPixels;
        }

        public int ExecuteConstraint(int pixelValue)
        {
            return pixelValue > maxPixels ? maxPixels : pixelValue;
        }
    }
}
