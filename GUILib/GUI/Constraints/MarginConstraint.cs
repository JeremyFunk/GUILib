using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.Constraints
{
    public class MarginConstraint : Constraint
    {
        private APixelConstraint pixels, minPixels;

        public MarginConstraint(APixelConstraint pixels, APixelConstraint minPixels = null)
        {
            this.pixels = pixels;
            this.minPixels = minPixels;
        }

        public void SetPixels(int pixels)
        {
            this.pixels = pixels;
        }

        public APixelConstraint GetPixels()
        {
            return pixels;
        }

        public int ExecuteConstraint(int parentSize, int selfSize)
        {
            if(minPixels == null)
                return parentSize - pixels.GetPixelValue(parentSize) - selfSize;
            return Math.Max(parentSize - pixels.GetPixelValue(parentSize) - selfSize, minPixels.GetPixelValue(parentSize));
        }
    }
}
