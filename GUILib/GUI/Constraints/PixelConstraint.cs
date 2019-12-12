using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib.GUI.Constraints
{
    class PixelConstraint : APixelConstraint
    {
        private int pixel;

        public PixelConstraint(int pixel)
        {
            this.pixel = pixel;
        }

        public override void ChangeValueByPixels(int value)
        {
            pixel += value;
        }

        public override int GetPixelValue(int size)
        {
            return pixel;
        }
    }
}
