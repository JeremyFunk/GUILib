using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    public class SinTransition : Transition
    {
        public override float GetCalculatedResult(float factor)
        {
            return (float)(Math.Sin(factor * Math.PI / 2));
        }
    }
}
