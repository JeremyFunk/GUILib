using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    class InvSinTransition : Transition
    {
        public override float GetCalculatedResult(float factor)
        {
            return 1 - (float)Math.Sin(((1 - factor) * Math.PI / 2));
        }
    }
}
