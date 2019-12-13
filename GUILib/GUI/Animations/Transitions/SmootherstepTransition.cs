using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    class SmootherstepTransition : Transition
    {
        public override float GetCalculatedResult(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
    }
}
