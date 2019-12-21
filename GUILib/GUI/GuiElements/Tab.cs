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
    class Tab : GuiElement
    {
        private List<GuiElement> dataChilds = new List<GuiElement>();

        private Text textElement;
        private BorderedQuad tabQuad;

        private bool active = false;

        public Tab(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
                fillMaterial = Theme.defaultTheme.GetTabFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetTabEdgeMaterial();
            if (fontSize < 0)
                fontSize = 0.8f;

            tabQuad = new BorderedQuad(0, 0, width, height, fillMaterial, edgeMaterial, Theme.defaultTheme.GetTabEdgeSize());

            tabQuad.mouseButtonPressedEvent = MousePressed;
            tabQuad.hoverEvent = Hover;
            tabQuad.endHoverEvent = EndHover;

            AddChild(tabQuad);
            if (text != "")
            {
                textElement = new Text(0, 0, text, fontSize);
                textElement.xConstraints.Add(new CenterConstraint());
                textElement.yConstraints.Add(new CenterConstraint());
                AddChild(textElement);
            }

            defaultMaterial = fillMaterial;
        }

        private void EndHover(MouseEvent e, GuiElement el)
        {
            if(!active)
                tabQuad.SetFillMaterial(defaultMaterial);
            else
                tabQuad.SetFillMaterial(Theme.defaultTheme.GetTabActiveMaterial());
        }

        private void Hover(MouseEvent e, GuiElement el)
        {
            if(!active)
                tabQuad.SetFillMaterial(Theme.defaultTheme.GetTabHoveredMaterial());
        }

        private void MousePressed(MouseEvent e, GuiElement el)
        {
            if(e.leftButtonDown)
                tabQuad.SetFillMaterial(Theme.defaultTheme.GetTabClickedMaterial());
        }

        public void SetTextColor(Vector4 color)
        {
            textElement.color = color;
        }

        public void Activate()
        {
            active = true;
            tabQuad.SetFillMaterial(Theme.defaultTheme.GetTabActiveMaterial());
        }
        public void Deactivate()
        {
            active = false;
            tabQuad.SetFillMaterial(defaultMaterial);
        }
    }
}
