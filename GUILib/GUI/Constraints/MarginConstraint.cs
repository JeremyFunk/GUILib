using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class MarginConstraint : Constraint
    {
        private int pixels, minPixels;
        private float pixelsF;
        private bool useInt;

        public MarginConstraint(int pixels)
        {
            this.pixels = pixels;
            useInt = true;
        }

        public MarginConstraint(float pixels, int minPixels = 0)
        {
            this.pixelsF = pixels;
            this.minPixels = minPixels;
            useInt = false;
        }

        public int ExecuteConstraint(int parentSize, int selfSize)
        {
            if(useInt)
                return parentSize - pixels - selfSize;
            return Math.Max((int)(parentSize - parentSize * pixelsF - selfSize), minPixels);
        }
    }
}
