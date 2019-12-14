using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class CenterConstraint : Constraint
    {
        public int ExecuteConstraint(int size, int parentSize)
        {
            return (parentSize - size) / 2;
        }
    }
}
