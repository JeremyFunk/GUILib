using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.GuiElements
{
    class VerticalList : GuiElement
    {
        private int padding;
        private Dictionary<int, GuiElement> elements = new Dictionary<int, GuiElement>();

        public VerticalList(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, int padding, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.padding = padding;
        }

        public void AddElement(GuiElement element)
        {
            if(elements.Count > 0)
            {
                int offset = element.curHeight + padding;

                SetHeight(curHeight + offset);

                int lastKey = elements.Keys.Last();

                foreach(GuiElement curElement in elements.Values)
                {
                    curElement.SetY(curElement.curY + offset);
                }

                element.SetY(0);

                elements.Add(lastKey + 1, element);
            }
            else
            {
                SetHeight(element.curHeight);
                element.SetY(0);

                elements.Add(0, element);
            }


            AddChild(element);
        }
    }
}
