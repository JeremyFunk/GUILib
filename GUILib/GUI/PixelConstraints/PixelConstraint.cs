using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.PixelConstraints
{
    public class PixelConstraint : APixelConstraint
    {
        private int pixel;

        public PixelConstraint(int pixel)
        {
            this.pixel = pixel;
        }

        public override int GetPixelValue(int size)
        {
            return pixel;
        }

        public int GetPixel()
        {
            return pixel;
        }
    }
}
