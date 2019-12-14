using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class MarginConstraint : Constraint
    {
        private int pixels;

        public MarginConstraint(int pixels)
        {
            this.pixels = pixels;
        }

        public int ExecuteConstraint(int parentSize, int selfSize)
        {
            return parentSize - pixels - selfSize;
        }
    }
}
