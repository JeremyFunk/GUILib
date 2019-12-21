using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class MirrorConstraint : Constraint
    {
        public MirrorConstraint()
        {
        }

        public int ExecuteConstraint(int parentSize, int selfSize)
        {
            return parentSize - selfSize;
        }
    }
}
