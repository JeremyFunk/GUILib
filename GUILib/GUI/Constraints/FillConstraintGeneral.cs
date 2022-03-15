using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    public class FillConstraintGeneral : GeneralConstraint
    {
        public override void ExecuteConstraint(int x, int y, int width, int height, int parentWidth, int parentHeight, out int oX, out int oY, out int oWidth, out int oHeight)
        {
            oX = 0;
            oY = 0;
            oWidth = parentWidth;
            oHeight = parentHeight;
        }
    }
}