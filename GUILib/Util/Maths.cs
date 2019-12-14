using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GUILib.Util
{
    static class MathsGeometry
    {
        public static bool IsInsideQuad(Vector2 p, float smallestX, float smallestY, float biggestX, float biggestY)
        {
            if (p.X < smallestX || p.X > biggestX || p.Y < smallestY || p.Y > biggestY)
                return false;
            return true;
        }

        public static bool IsInsideQuad(Vector2 p, GUI.GuiElements.GuiElement element)
        {
            if (!element.IsAnimationRunning()) { 
                if (p.X < element.curX + element.animationOffsetX || p.X > element.curX + element.curWidth + element.animationOffsetX || p.Y < element.curY + element.animationOffsetY || p.Y > element.curY + element.curHeight + element.animationOffsetY)
                    return false;
            }else
                if (p.X < element.curX || p.X > element.curX + element.curWidth || p.Y < element.curY || p.Y > element.curY + element.curHeight)
                    return false;
            return true;
        }
    }
}
