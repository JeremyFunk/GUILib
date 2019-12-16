using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    abstract class APixelConstraint
    {
        //Size will be the width or height of the parent element 
        public abstract int GetPixelValue(int size);
        public abstract void ChangeValueByPixels(int value);

        public static implicit operator APixelConstraint(int value) => new PixelConstraint(value);
        public static implicit operator APixelConstraint(float value) => new PixelSizeConstraint(value);
    }
}
