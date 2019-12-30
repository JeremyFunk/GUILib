using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Animations.Transitions
{

    ///<summary>Implements the Catmull-Rom-Spline function.</summary>
    class CatmullRomSplineTransition : Transition
    {
        private float p0, p3;

        public CatmullRomSplineTransition(float p0, float p3)
        {
            this.p0 = p0;
            this.p3 = p3;
        }

        public override float GetCalculatedResult(float factor)
        {
            factor = Catmullrom(factor, p0, 0, 1, p3);
            return factor;
        }

        private float Catmullrom(float t, float p0, float p1, float p2, float p3)
        {
            return 0.5f * (
                          (2 * p1) +
                          (-p0 + p2) * t +
                          (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t +
                          (-p0 + 3 * p1 - 3 * p2 + p3) * t * t * t
                          );
        }
    }
}
