using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    class SmoothstepTransition : Transition
    {
        int amount;

        public SmoothstepTransition(int amount)
        {
            this.amount = amount;
        }

        /**
         * amount is a number between 1 and 3. 1 Indicates the smallest smoothing effect, while 3 indicates the greatest.
         */
        public override float GetCalculatedResult(float t)
        {
            if(amount == 1)
                return (t * t * (3 - 2 * t));
            if(amount == 2)
                return t * t * t * (t * (t * 6 - 15) + 10);
            else
            {
                float xsq = t * t; 
                float xsqsq = xsq * xsq; 
                return xsqsq * (25.0f - 48.0f * t + xsq * (25.0f - xsqsq));
            }
        }
    }
}
