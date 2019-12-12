using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.Util;

namespace GuiLib.GUI.Constraints
{
    class PixelSizeConstraint : APixelConstraint
    {
        private float size;

        public PixelSizeConstraint(float size)
        {
            this.size = size;
        }

        public override void ChangeValueByPixels(int value)
        {
            size += value / GameSettings.Width;
        }

        public override int GetPixelValue(int size)
        {
            return (int)Math.Round(size * this.size);
        }
    }
}
