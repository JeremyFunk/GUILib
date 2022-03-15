using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.Util;

namespace GUILib.GUI.PixelConstraints
{
    public class PixelSizeConstraint : APixelConstraint
    {
        private float size;

        public PixelSizeConstraint(float size)
        {
            this.size = size;
        }

        public override int GetPixelValue(int size)
        {
            return (int)Math.Round(size * this.size);
        }

        public float GetPixel()
        {
            return size;
        }
    }
}
