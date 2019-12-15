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

namespace GUILib.GUI.GuiElements
{
    class VerticalList : GuiElement
    {
        private int padding;
        private Dictionary<int, GuiElement> elements = new Dictionary<int, GuiElement>();

        public VerticalList(float x, float y, float width, float height, int padding, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.padding = padding;
        }

        public override void MouseEventElement(MouseEvent events)
        {}

        public override void KeyEvent(KeyEvent events)
        {}

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale, float opacity)
        {}

        public override void UpdateElement(float delta)
        {}

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
