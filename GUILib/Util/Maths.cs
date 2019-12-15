﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GUILib.GUI.GuiElements;

namespace GUILib.Util
{
    static class Utility
    {
        public static List<GuiElement> GetZIndexSorted(List<GuiElement> elements)
        {
            elements.Sort((e1, e2) => e1.zIndex.CompareTo(e2.zIndex));

            return elements;
        }
    }
    static class MathsGeometry
    {
        public static bool IsInsideQuad(Vector2 p, float smallestX, float smallestY, float biggestX, float biggestY)
        {
            if (p.X < smallestX || p.X > biggestX || p.Y < smallestY || p.Y > biggestY)
                return false;
            return true;
        }

        public static bool IsInsideQuad(Vector2 p, GuiElement element)
        {
            if (!element.IsAnimationRunning()) { 
                if (p.X < element.curX + element.animationOffsetX || p.X > element.curX + element.curWidth + element.animationOffsetWidth + element.animationOffsetX || p.Y < element.curY + element.animationOffsetY || p.Y > element.curY + element.curHeight + element.animationOffsetHeight + element.animationOffsetY)
                    return false;
            }else
                if (p.X < element.curX || p.X > element.curX + element.curWidth || p.Y < element.curY || p.Y > element.curY + element.curHeight)
                    return false;
            return true;
        }
    }
}
