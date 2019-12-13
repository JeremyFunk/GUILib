using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    public abstract class Transition
    {
        public readonly static Transition defaultTransition = new LinearTransition();
        public abstract float GetCalculatedResult(float factor);
    }
}
