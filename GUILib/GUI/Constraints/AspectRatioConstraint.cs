using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class AspectRatioConstraint : Constraint
    {
        private int value;
        public AspectRatioConstraint(int value)
        {
            this.value = value;
        }

        public int ExecuteConstraint(int size)
        {
            return size + value;
        }
    }
}
