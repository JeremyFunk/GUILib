using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GUILib.GUI.GuiElements;

namespace GUILib.Util
{
    static public class MathsUtil
    {
        public static List<GuiElement> GetZIndexSorted(List<GuiElement> elements)
        {
            elements.Sort((e1, e2) => e1.ZIndex.CompareTo(e2.ZIndex));

            return elements;
        }
    }
    static public class MathsGeometry
    {
        public static bool IsInsideQuad(Vector2 p, GuiElement element)
        {
            if (!element.IsAnimationRunning()) { 
                if (p.X < element.curX + element.animationOffsetX || p.X > (element.curX + element.curWidth + element.animationOffsetX + element.animationOffsetWidth) * element.absoluteScale || p.Y < element.curY + element.animationOffsetY || p.Y > element.curY + element.curHeight + element.animationOffsetY + element.animationOffsetHeight)
                    return false;
            }else
                if (p.X < element.curX || p.X > (element.curX + element.curWidth) * element.absoluteScale || p.Y < element.curY || p.Y > element.curY + element.curHeight)
                    return false;
            return true;
        }
    }
}
