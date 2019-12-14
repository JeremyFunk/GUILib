using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    class InvertPowersTransition : Transition
    {
        private int amount;

        public InvertPowersTransition(int amount)
        {
            this.amount = amount;
        }

        public override float GetCalculatedResult(float factor)
        {
            for(int i = 0; i < amount; i++)
            {
                factor = 1 - (1 - factor) * (1 - factor);
            }

            return factor;
        }
    }
}
