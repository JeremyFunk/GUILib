using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    public class FixConstraint : Constraint
    {
        private int pixels;

        public FixConstraint(int pixels)
        {
            this.pixels = pixels;
        }

        public int ExecuteConstraint(int pixelValue)
        {
            return pixels;
        }
    }
}
