using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    class SmoothstepTransition : Transition
    {
        public override float GetCalculatedResult(float t)
        {
            return (t * t * (3 - 2 * t));
        }
    }
}
