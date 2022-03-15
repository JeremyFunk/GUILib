using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{
    public class PowersTransition : Transition
    {
        private int amount;

        public PowersTransition(int amount)
        {
            this.amount = amount;
        }

        public override float GetCalculatedResult(float factor)
        {
            for (int i = 0; i < amount; i++)
            {
                factor = factor * factor;
            }

            return factor;
        }
    }
}
