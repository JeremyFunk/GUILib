using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.PixelConstraints
{
    public class CombinedSubtractConstraint : APixelConstraint
    {
        APixelConstraint c1, c2;

        public CombinedSubtractConstraint(APixelConstraint c1, APixelConstraint c2)
        {
            this.c1 = c1;
            this.c2 = c2;
        }

        public override int GetPixelValue(int size)
        {
            return c1.GetPixelValue(size) - c2.GetPixelValue(size);
        }
    }
}
