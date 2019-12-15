using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Constraints
{
    class AddConstraint : Constraint
    {
        private int value;
        public AddConstraint(int value)
        {
            this.value = value;
        }

        public int ExecuteConstraint(int size)
        {
            return size + value;
        }
    }
}
