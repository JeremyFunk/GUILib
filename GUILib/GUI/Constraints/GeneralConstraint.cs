using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    public abstract class GeneralConstraint
    {
        public abstract void ExecuteConstraint(int x, int y, int width, int height, int parentWidth, int parentHeight, out int oX, out int oY, out int oWidth, out int oHeight);
    }
}
