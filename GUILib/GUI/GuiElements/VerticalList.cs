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
        private bool usesSeperator;
        private Dictionary<int, GuiElement> elements = new Dictionary<int, GuiElement>();

        public VerticalList(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, int padding, bool usesSeperator = false, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.usesSeperator = usesSeperator;
            this.padding = padding;
            //Random r = new Random();  Quad q = new Quad(new Material(new Vector4((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble())), 0, 0, 0, 0); q.generalConstraint = new FillConstraintGeneral(); AddChild(q);
        }

        bool addSeperatorRun = false;
        public override void AddChild(GuiElement element)
        {
            if(elements.Count > 0)
            {
                if (usesSeperator && !addSeperatorRun)
                {
                    addSeperatorRun = true;

                    Quad seperator = new Quad(10, 5, 1f, 2, Theme.defaultTheme.GetSeperatorMaterial());
                    seperator.widthConstraints.Add(new SubtractConstraint(20));

                    AddChild(seperator);
                }
                else
                {
                    addSeperatorRun = false;
                }

                int offset = element.curHeight + padding;

                SetHeight(curHeight + offset);

                int lastKey = elements.Keys.Last();

                foreach(GuiElement curElement in elements.Values)
                {
                    curElement.SetY(curElement.curY + offset);
                }

                element.SetY(0);

                SetY(curY - offset);

                elements.Add(lastKey + 1, element);
            }
            else
            {
                SetHeight(element.curHeight);
                SetY(curY - element.curHeight);
                element.SetY(0);

                elements.Add(0, element);
            }

            base.AddChild(element);
        }
    }
}
