using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    class LinearTransition : Transition
    {
        public override float GetCalculatedResult(float factor)
        {
            return factor;
        }
    }
}
