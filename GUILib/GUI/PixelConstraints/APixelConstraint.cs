using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.PixelConstraints
{
    public abstract class APixelConstraint
    {
        //Size will be the width or height of the parent element 
        public abstract int GetPixelValue(int size);

        public static implicit operator APixelConstraint(int value) => new PixelConstraint(value);
        public static implicit operator APixelConstraint(float value) => new PixelSizeConstraint(value);

        public static APixelConstraint operator -(APixelConstraint c1, APixelConstraint c2)
        {
            return new CombinedSubtractConstraint(c1, c2);
        }
    }
}
