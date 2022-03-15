using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    public class MarginConstraintGeneral : GeneralConstraint
    {
        private readonly int mX, mY, mW, mH;
        public MarginConstraintGeneral(int mX, int mY, int mW, int mH)
        {
            this.mX = mX;
            this.mY = mY;
            this.mW = mW;
            this.mH = mH;
        }

        public MarginConstraintGeneral(int m)
        {
            this.mX = mY = mW = mH = m;
        }

        public override void ExecuteConstraint(int x, int y, int width, int height, int parentWidth, int parentHeight, out int oX, out int oY, out int oWidth, out int oHeight)
        {
            oX = mX;
            oY = mY;
            oWidth = parentWidth - mX * 2;
            oHeight = parentHeight - mY * 2;
        }
    }
}